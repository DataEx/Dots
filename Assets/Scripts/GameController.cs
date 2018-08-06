using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameController : MonoBehaviour {

    // Collection of dots currently chained together
    List<Dot> dotsInteracting = new List<Dot>();

    const int MIN_DOTS_NEEDED_TO_CHAIN = 2;

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
        if(Input.GetKeyDown(KeyCode.Q))
            EditorApplication.isPaused = true;


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
            if (dotsInteracting.Count >= MIN_DOTS_NEEDED_TO_CHAIN)
            {
                // If create square, remove all dots of that color
                if (IsSquareCreated())
                {
                    List<Dot> dotsToDelete = Globals.Grid.GetAllDotsOfColor(MatchingColor);
                    Globals.Grid.RemoveDots(dotsToDelete);
                }
                // Otherwise, remove the dots that have been selected
                else
                {
                    Globals.Grid.RemoveDots(dotsInteracting);
                }
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
        // Don't interact with any dot that is currently moving
        if (hitDot.GridBox.IsUpdatingCoordinate)
            return;

        // If set is empty, add it
        if (dotsInteracting.Count == 0)
        {
            dotsInteracting.Add(hitDot);
            MatchingColor = hitDot.Color;
            DrawCursorChain(hitDot);
        }
        else if (IsValidDot(hitDot))
        {
            // If same dot as previous hit, undo chain
            if (hitDot == PreviousChainHead)
            {
                PreviousChainHead.RemoveChain(LastDotHit);
                dotsInteracting.RemoveAt(LastIndexOfDotsInteracting);
            }
            // Add Chain
            else
            {
                Chain newChain = CreateChain();
                if (newChain != null)
                {
                    newChain.Initialize(LastDotHit);
                    newChain.ConnectTo(hitDot);
                    LastDotHit.AddChain(newChain);
                }

                dotsInteracting.Add(hitDot);
            }
            DrawCursorChain(hitDot);
        }
    }

    private bool IsValidDot(Dot hitDot)
    {
        return 
            hitDot.Color == MatchingColor &&
            hitDot.Coordinate.IsNeighbor(LastDotHit.Coordinate) && 
            (hitDot == PreviousChainHead || !LastDotHit.IsConnectedTo(hitDot)) && 
            LastDotHit.CanAddAnotherChain();
    }

    private void DrawCursorChain(Dot hitDot)
    {
        if (cursorChain == null)
            cursorChain = CreateChain();

        if (cursorChain != null)
            cursorChain.Initialize(hitDot);
    }


    Chain CreateChain()
    {
        if (Globals.ChainPrefab != null)
            return Instantiate(Globals.ChainPrefab);
        else
        {
            Debug.LogError("No valid Chain Prefab");
            return null;
        }

    }


    // Check is square is created by determining if the same Dot has been reached more than once
    private bool IsSquareCreated()
    {
        HashSet<Dot> dotSet = new HashSet<Dot>();
        foreach (Dot dot in dotsInteracting)
        {
            if (dotSet.Contains(dot))
                return true;
            else
                dotSet.Add(dot);
        }

        return false;
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






