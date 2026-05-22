using System;
using System.Numerics;
using Lumina.Data;
using Lumina.Data.Parsing;

namespace SpakethPlugin.Windows;

public static class Colors
{
    public static readonly Vector4 Green = _makeColor(45, 150, 45, 255);
    public static readonly Vector4 Red = _makeColor(189, 50, 50, 255);
    public static readonly Vector4 Purple = _makeColor(190, 60, 170, 255);

    private static Vector4 _makeColor(uint x, uint y, uint z, uint w)
    {
        return new Vector4()
        {
            X = (float)x / 0xFF,
            Y = (float)y / 0xFF,
            Z = (float)z / 0xFF,
            W = (float)w / 0xFF
        };
    }
}
