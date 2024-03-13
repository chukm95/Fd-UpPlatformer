using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace FuckedUpPlatformer.Graphics.Shapes
{
    internal static class ShapeGenerator
    {
        private const int MIN_POINTS_CIRCLE = 4;
        private const int MAX_POINTS_CIRCLE = 1600;

        private const string QUAD_CENTERED_FILL = "QUAD_CENTERED_FILL";
        private const string CIRLCE_CENTERED_FILL = "CIRLCE_CENTERED__FILL_P";
        private static Dictionary<string, Shape> _generatedShapes = new Dictionary<string, Shape>();

        public static Shape GenerateCenteredQuad() {
            if (_generatedShapes.ContainsKey(QUAD_CENTERED_FILL))
                return _generatedShapes[QUAD_CENTERED_FILL];

            Vector3[] positions = new Vector3[]
            {
                new Vector3(-0.5f,  0.5f, 0),
                new Vector3(0.5f,  0.5f, 0),
                new Vector3(0.5f, -0.5f, 0),
                new Vector3(-0.5f, -0.5f, 0)
            };

            Vector2[] uvs = new Vector2[]
            {
                new Vector2(0, 0),
                new Vector2(1, 0),
                new Vector2(1, 1),
                new Vector2(0, 1)
            };

            uint[] indices = new uint[]
            {
                0, 1, 2, 0, 2, 3
            };

            Shape shape = new Shape(positions, uvs, indices);
            _generatedShapes.Add(QUAD_CENTERED_FILL, shape);

            return shape;
        }

        public static Shape GenerateCenteredCircle(int points) {
            points = Math.Clamp(points, MIN_POINTS_CIRCLE, MAX_POINTS_CIRCLE);

            string shapeName = $"{CIRLCE_CENTERED_FILL}{points}";
            if (_generatedShapes.ContainsKey(shapeName))
                return _generatedShapes[shapeName];

            int numOfPoints = points + 1;
            int numOfIndices = points * 3;

            Vector3[] positions = new Vector3[numOfPoints];
            Vector2[] uvs = new Vector2[numOfPoints];
            uint[] indices = new uint[numOfIndices];

            uvs[points].X = 0.5f;
            uvs[points].Y = 0.5f;

            float radiansPerTurn = (360f * (MathF.PI / 180)) / points;
            float uvOffset = 0.5f;

            for (int i = 0; i < points; i++) {
                positions[i].X = MathF.Cos(radiansPerTurn * i) * 0.5f;
                positions[i].Y = MathF.Sin(radiansPerTurn * i) * 0.5f;

                uvs[i].X = (MathF.Cos(radiansPerTurn * i) * 0.5f) + uvOffset;
                uvs[i].Y = (MathF.Sin(radiansPerTurn * i) * -0.5f) + uvOffset;
            }

            for (int i = 0; i < points; i++) {
                int index = i * 3;
                indices[index] = 0;
                indices[index + 1] = (uint)i + 1;
                indices[index + 2] = (uint)((i + 2) % points);
            }

            

            Shape shape = new Shape(positions, uvs, indices);
            _generatedShapes.Add(shapeName, shape);

            return shape;
        }

    }
}