using UnityEngine;
using UnityEngine.UI;

/*
Modified by Haoze in 2024
Original work: Radial Layout Group by Just a Pixel (Danny Goodayle) - http://www.justapixel.co.uk

MIT License

Copyright (c) 2015

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

/// <summary>
/// A custom layout group that arranges child UI elements in a circular (radial) layout.
/// </summary>
public class RadialLayoutGroup : LayoutGroup
{
    [Tooltip("Distance from the center to the child elements (radius of the circle).")]
    public float distance = 100f;

    [Tooltip("Minimum angle for the layout range.")]
    [Range(0f, 360f)] public float minAngle = 0f;

    [Tooltip("Maximum angle for the layout range.")]
    [Range(0f, 360f)] public float maxAngle = 360f;

    [Tooltip("Starting angle for the first child element.")]
    [Range(0f, 360f)] public float startAngle = 0f;

    private bool layoutDirty = true;

    /// <summary>
    /// Called when the script instance is enabled.
    /// Initializes the radial layout.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        CalculateRadial();
    }

    /// <summary>
    /// Override method for horizontal layout calculation.
    /// Not used in this layout group.
    /// </summary>
    public override void SetLayoutHorizontal() { }

    /// <summary>
    /// Override method for vertical layout calculation.
    /// Not used in this layout group.
    /// </summary>
    public override void SetLayoutVertical() { }

    /// <summary>
    /// Calculates the layout when horizontal input changes.
    /// </summary>
    public override void CalculateLayoutInputHorizontal()
    {
        if (layoutDirty) CalculateRadial();
    }

    /// <summary>
    /// Calculates the layout when vertical input changes.
    /// </summary>
    public override void CalculateLayoutInputVertical()
    {
        if (layoutDirty) CalculateRadial();
    }

    /// <summary>
    /// Called when child objects change. Marks the layout as dirty and recalculates the layout.
    /// </summary>
    protected override void OnTransformChildrenChanged()
    {
        layoutDirty = true;
        CalculateRadial();
    }

    /// <summary>
    /// Calculates the positions of the child elements in a radial layout.
    /// </summary>
    private void CalculateRadial()
    {
        // Clear the previous tracker data to prevent unnecessary control of child elements
        m_Tracker.Clear();

        // If there are no child elements, exit the method
        int childCount = transform.childCount;
        if (childCount == 0) return;

        // Calculate the angle offset between each child element
        float offsetAngle = (maxAngle - minAngle) / Mathf.Max(1, childCount - 1);
        float angle = startAngle;

        // Precompute the Cos and Sin values for each angle
        float[] cosValues = new float[childCount];
        float[] sinValues = new float[childCount];
        for (int i = 0; i < childCount; i++)
        {
            cosValues[i] = Mathf.Cos(angle * Mathf.Deg2Rad);
            sinValues[i] = Mathf.Sin(angle * Mathf.Deg2Rad);
            angle += offsetAngle;
        }

        // Set the position and properties of each child element
        for (int i = 0; i < childCount; i++)
        {
            RectTransform child = (RectTransform)transform.GetChild(i);
            if (child == null) continue;

            // Add the child to the tracker to prevent user modifications
            m_Tracker.Add(this, child, DrivenTransformProperties.Anchors | DrivenTransformProperties.AnchoredPosition | DrivenTransformProperties.Pivot);

            // Reset the child's anchors, pivot, and position
            child.anchorMin = child.anchorMax = new Vector2(0.5f, 0.5f);
            child.pivot = new Vector2(0.5f, 0.5f);
            child.anchoredPosition = Vector2.zero;

            // Calculate the position of the child element
            Vector2 position = new Vector2(cosValues[i], sinValues[i]) * distance;
            child.anchoredPosition = position;
        }

        // Mark the layout as clean after calculation
        layoutDirty = false;
    }
}
