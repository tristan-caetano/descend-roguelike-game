using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Edgar.Unity.Editor
{
    public class ConnectionNode
    {
        public ConnectionBase Connection { get; }

        public RoomNode From { get; }

        public RoomNode To { get; }

        public ConnectionNode(ConnectionBase connection, RoomNode from, RoomNode to)
        {
            Connection = connection;
            From = from;
            To = to;
        }

        public void Draw(float zoom, Vector2 panOffset, bool isDirected)
        {
            var style = Connection.GetEditorStyle(Selection.activeObject == Connection);

            var oldColor = Handles.color;

            var from = From.GetRect(zoom, panOffset).center;
            var to = To.GetRect(zoom, panOffset).center;

            Handles.color = style.LineColor;
            Handles.DrawLine(From.GetRect(zoom, panOffset).center, To.GetRect(zoom, panOffset).center);

            Handles.color = style.HandleBackgroundColor;
            var center = (from + to) / 2;
            var direction = to - from;
            direction.Normalize();
            direction *= 5 * zoom;
            var perpendicular = Vector2.Perpendicular(direction);

            var points = new List<Vector3>();
            points.Add(center - direction - perpendicular);
            points.Add(center + direction - perpendicular);
            points.Add(center + direction + perpendicular);
            points.Add(center - direction + perpendicular);
            Handles.DrawAAConvexPolygon(points.ToArray());

            if (isDirected)
            {
                center += 1.2f * direction;
                Handles.DrawAAConvexPolygon(center - perpendicular, center + direction, center + perpendicular);
            }

            Handles.color = oldColor;
        }

        public Rect GetHandleRect(float zoom, Vector2 panOffset)
        {
            var width = zoom * 12;

            var handleCenter = Vector2.Lerp(From.GetRect(zoom, panOffset).center, To.GetRect(zoom, panOffset).center, 0.5f);
            var rect = new Rect(handleCenter.x - width / 2.0f, handleCenter.y - width / 2.0f, width, width);

            return rect;
        }
    }
}