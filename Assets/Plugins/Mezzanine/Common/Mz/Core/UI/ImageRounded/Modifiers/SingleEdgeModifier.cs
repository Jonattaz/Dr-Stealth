using UnityEngine;
using Mz.App.UI;

public class SingleEdgeModifier : ImageRoundedModifier {
    public SingleEdgeModifier(float radius, EdgeType edge)
    {
        Radius = radius;
        Edge = edge;
    }
    
    public float Radius { get; set; }
    public EdgeType Edge { get; set; }

    public enum EdgeType {
        Top,
        Bottom,
        Left,
        Right
    }

    public override Vector4 CalculateRadius (Rect imageRect){
        switch (Edge) {
            case EdgeType.Top:
                return new Vector4(Radius,Radius,0,0);
            case EdgeType.Right:
                return new Vector4(0,Radius,Radius,0);
            case EdgeType.Bottom:
                return new Vector4(0,0,Radius,Radius);
            case EdgeType.Left:
                return new Vector4(Radius,0,0,Radius);
            default:
                return new Vector4(0,0,0,0);
        }
    }
}