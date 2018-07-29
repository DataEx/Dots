using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Collection of dots currently chained together
    List<Dot> dotsInteracting = new List<Dot>();

    Dot LastDotHit
    {
        get {
            if (dotsInteracting == null || dotsInteracting.Count == 0)
                return null;
            else
                return dotsInteracting[LastIndexOfDotsInteracting];
        }
    }

    int LastIndexOfDotsInteracting
    {
        get {
            return dotsInteracting.Count - 1;
        }
    }

    // The Dot that owns the most recently added Chain
    Dot PreviousChainHead
    {
        get
        {
            if (dotsInteracting == null || dotsInteracting.Count <= 1)
                return null;
            else
            {
                // Want second to last most recently added
                return dotsInteracting[LastIndexOfDotsInteracting - 1];
            }
        }
    }

    DotColor MatchingColor;

    // Chain drawn from last dot to cursor 
    Chain cursorChain = null;

    void Update ()
    {

        if (IsTouchingScreen())
        {
            RaycastHit hit;
            Ray ray = GetRayFromMousePosition();
            // If touching dot, interact with it
            if (Physics.Raycast(ray, out hit))
            {
                Dot hitDot = hit.collider.GetComponent<Dot>();
                if (hitDot != null && hitDot != LastDotHit)
                {
                    TryAddDot(hitDot);
                }
            }
            if (cursorChain != null)
                cursorChain.UpdateTransform(ray.origin);

        }
        // No longer interacting with screen
        else
        {
            if (dotsInteracting.Count >= 2)
            {
                Globals.Grid.RemoveDots(dotsInteracting);
                Globals.Grid.RepopulateGrid();
            }
            Cleanup();
        }
    }


    // Isolate input
    private bool IsTouchingScreen()
    {
        return Input.GetMouseButton(0);
    }
    private Ray GetRayFromMousePosition()
    {
        return Camera.main.ScreenPointToRay(Input.mousePosition);
    }


    void TryAddDot(Dot hitDot)
    {
        // If set is empty, add it
        if (dotsInteracting.Count == 0)
        {
            dotsInteracting.Add(hitDot);
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
                Cleanup();
                List<Dot> dotsToDelete = Globals.Grid.GetAllDotsOfColor(MatchingColor);
                Globals.Grid.RemoveDots(dotsToDelete);
                Globals.Grid.RepopulateGrid();
            }

            else
            {
                // Add Chain
                Chain newChain = Instantiate(Globals.ChainPrefab);
                newChain.Initialize(LastDotHit);
                newChain.UpdateTransform(hitDot.transform.position);

                LastDotHit.Chain = newChain;

                dotsInteracting.Add(hitDot);
            }
            DrawcursorChain(hitDot);
        }
    }

    private void RemoveLastDot()
    {
        Dot prevDot = dotsInteracting[LastIndexOfDotsInteracting];
        dotsInteracting.RemoveAt(LastIndexOfDotsInteracting);
    }


    private void DrawcursorChain(Dot hitDot)
    {
        if (cursorChain == null)
        {
            cursorChain = Instantiate(Globals.ChainPrefab);
        }
        cursorChain.Initialize(hitDot);
    }

    // Reseting state, to be run after a move has been made
    private void Cleanup()
    {
        dotsInteracting.Clear();
        if (cursorChain != null)
        {
            cursorChain.DestroyChain();
        }
    }

}






