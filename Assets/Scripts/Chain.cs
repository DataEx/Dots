using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Chain : MonoBehaviour {

    // Dot where the chain is originating from
    Dot originDot = null;

    private Renderer rendererComponent;

    private void Awake()
    {
        rendererComponent = this.GetComponent<Renderer>();
    }

    public void Initialize(Dot dot)
    {
        originDot = dot;

        if (dot != null)
            rendererComponent.material = dot.Color.MaterialColor;
        else
            rendererComponent.material = null;
    }
    
    // Connects a dot with either another dot or the cursor's location
    public void UpdateTransform(Vector3 endPoint)
    {
        if (originDot == null)
            return;

        endPoint.z = originDot.transform.position.z;

        // Position self between both objects
        this.transform.position = (originDot.transform.position + endPoint) / 2f;

        // Orient self appropriately
        this.transform.LookAt(originDot.transform);

        // Move in front of dots so that the chain will always be drawn on top of the dots
        this.transform.position -= Vector3.forward;

        // Scale chain between two objs (z-component only)
        float objDistance = Vector3.Distance(originDot.transform.position, endPoint);
        Vector3 chainScale = this.transform.localScale;
        chainScale.z = objDistance;
        this.transform.localScale = chainScale;
    }

    public void DestroyChain()
    {
        Destroy(this.gameObject);
    }
}
