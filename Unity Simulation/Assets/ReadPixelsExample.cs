using UnityEngine;

public class ReadPixelsExample : MonoBehaviour
{
    // Set this reference to a GameObject that has a Renderer component,
    // and a material that displays a texure (such as the Default material).
    // A standard Cube or other primitive works for the purposes of this example.
    public Renderer screenGrabRenderer;

    private Texture2D destinationTexture;
    private bool isPerformingScreenGrab;

    void Start()
    {
        // Create a new Texture2D with the width and height of the screen, and cache it for reuse
        destinationTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

        // Make screenGrabRenderer display the texture.
        screenGrabRenderer.material.mainTexture = destinationTexture;

        // Add the onPostRender callback
        Camera.onPostRender += OnPostRenderCallback;
    }

    void Update()
    {
        // When the user presses the space key, perform the screen grab operation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPerformingScreenGrab = true;
        }
    }

    void OnPostRenderCallback(Camera cam)
    {
        if (isPerformingScreenGrab)
        {
            // Check whether the Camera that has just finished rendering is the one you want to take a screen grab from
            if (cam == Camera.main)
            {
                // Define the parameters for the ReadPixels operation
                Rect regionToReadFrom = new Rect(0, 0, Screen.width, Screen.height);
                int xPosToWriteTo = 0;
                int yPosToWriteTo = 0;
                bool updateMipMapsAutomatically = false;

                // Copy the pixels from the Camera's render target to the texture
                destinationTexture.ReadPixels(regionToReadFrom, xPosToWriteTo, yPosToWriteTo, updateMipMapsAutomatically);

                // Upload texture data to the GPU, so the GPU renders the updated texture
                // Note: This method is costly, and you should call it only when you need to
                // If you do not intend to render the updated texture, there is no need to call this method at this point
                destinationTexture.Apply();

                // Reset the isPerformingScreenGrab state
                isPerformingScreenGrab = false;
            }
        }
    }

    // Remove the onPostRender callback
    void OnDestroy()
    {
        Camera.onPostRender -= OnPostRenderCallback;
    }
}