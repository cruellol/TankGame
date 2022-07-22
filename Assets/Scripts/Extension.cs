using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tanks
{
    public static class Extension
    {
        private static Dictionary<DirectionType, Vector3> _rotations;
        private static Dictionary<DirectionType, Vector3> _directions;

        static Extension()
        {
            _rotations = new Dictionary<DirectionType, Vector3>() {
                {DirectionType.Left,new Vector3(0f,0f,90f) },
                {DirectionType.Top,new Vector3(0f,0f,0f) },
                {DirectionType.Right,new Vector3(0f,0f,270f) },
                {DirectionType.Bottom,new Vector3(0f,0f,180f) },
            };
            _directions = new Dictionary<DirectionType, Vector3>() {
                {DirectionType.Left,new Vector3(-1f,0f,0f) },
                {DirectionType.Top,new Vector3(0f,1f,0f) },
                {DirectionType.Right,new Vector3(1f,0f,0f) },
                {DirectionType.Bottom,new Vector3(0f,-1f,0f) },
            };
        }
        public static Vector3 GetVectorFromDirection(this DirectionType direction) => _directions[direction];
        public static DirectionType GetDirectionFromVector(this Vector3 vector) => _directions.First(d => d.Value == vector).Key;
        public static DirectionType GetDirectionFromVector(this Vector2 vector)
        {
            Vector3 dir = vector;
            return _directions.First(d => d.Value == dir).Key;
        }
        public static Vector3 GetVectorFromRotation(this DirectionType rotation) => _rotations[rotation];
        public static DirectionType GetRotationFromVector(this Vector3 vector) => _rotations.First(d => d.Value == vector).Key;
        public static DirectionType GetRotationFromVector(this Vector2 vector)
        {
            Vector3 rot = vector;
            return _rotations.First(d => d.Value == rot).Key;
        }
    }
    public enum DirectionType : byte
    {
        Error = 0,
        Left,
        Top,
        Right,
        Bottom
    }
    public enum SideType : byte
    {
        None,
        Player,
        Enemy
    }
}
