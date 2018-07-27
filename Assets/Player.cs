using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: rename Player class
//TODO: fix data structure use. Want O(1) insertion and O(1) search
//TODO: use object pool
public class Player : MonoBehaviour {

    // Both are the same collection of dots currently chained together
    HashSet<Dot> dotsInteracting = new HashSet<Dot>();
    Stack<Dot> chainedDots = new Stack<Dot>();

    Dot LastDotHit
    {
        get {
            if (chainedDots == null || chainedDots.Count == 0)
                return null;
            else
                return chainedDots.Peek();
        }
    }

    // See if this can be more optimized
    Dot PreviousChainHead
    {
        get
        {
            if (chainedDots == null || chainedDots.Count <= 1)
                return null;
            else
            {
                Dot topDot = chainedDots.Pop();
                Dot chainHead = chainedDots.Peek();
                chainedDots.Push(topDot);
                return chainHead;
            }
        }
    }

    DotColor MatchingColor;
    Chain cursorChain = null;

    void Update () {

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Dot hitDot = hit.collider.GetComponent<Dot>();
                if (hitDot != null && hitDot != LastDotHit)
                {
                    TryAddDot(hitDot);
                }
            }
            if(cursorChain != null)
                cursorChain.UpdateTransform(ray.origin);

        }
        // Let go of mouse
        else {
            if (dotsInteracting.Count >= 2)
            {
                // Remove dots from grid
                foreach (Dot dot in dotsInteracting)
                {
                    dot.DestroyDot();
                }
                Globals.Grid.RepopulateGrid();
            }

            CleanupMove();

            // TODO: Running every frame, see if can use delegate
        }
    }

    void TryAddDot(Dot hitDot)
    {
        // If set is empty, add it
        if (dotsInteracting.Count == 0)
        {
            AddDotToList(hitDot);
            MatchingColor = hitDot.Color;
            DrawcursorChain(hitDot);
        }
        else if (hitDot.Color == MatchingColor && hitDot.Coordinate.IsNeighbor(LastDotHit.Coordinate))
        {
            // If same dot as previous hit, undo chain
            if (hitDot == PreviousChainHead)
            {
                Chain chainToDelete = PreviousChainHead.Chain;
                RemoveLastDot();
                chainToDelete.DestroyChain();
            }

            // If already in set and not the PreviousChainHead, that means we've created a square-esque shape
            else if (dotsInteracting.Contains(hitDot))
            {
                CleanupMove();

                List<Dot> dotsToDelete = Globals.Grid.GetAllDotsOfColor(MatchingColor);
                foreach (Dot dot in dotsToDelete)
                {
                    dot.DestroyDot();
                }
                Globals.Grid.RepopulateGrid();
            }

            else
            {
                // AddChain
                Chain newChain = Instantiate(Globals.ChainPrefab);
                newChain.name = Time.time.ToString();
                newChain.Initialize(LastDotHit);
                newChain.UpdateTransform(hitDot.transform.position);
                LastDotHit.Chain = newChain;

                dotsInteracting.Add(hitDot);
                chainedDots.Push(hitDot);
            }

            DrawcursorChain(hitDot);
        }
    }

    private void RemoveLastDot()
    {
        Dot prevDot = chainedDots.Pop();
        dotsInteracting.Remove(prevDot);
    }

    private void AddDotToList(Dot hitDot)
    {
        dotsInteracting.Add(hitDot);
        chainedDots.Push(hitDot);
    }

    private void DrawcursorChain(Dot hitDot)
    {
        if (cursorChain == null)
        {
            cursorChain = Instantiate(Globals.ChainPrefab);
        }
        cursorChain.Initialize(hitDot);
    }

    private void CleanupMove()
    {
        dotsInteracting.Clear();
        chainedDots.Clear();
        if (cursorChain != null)
        {
            cursorChain.DestroyChain();
        }
    }


}






