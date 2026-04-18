using System;
using Shinrai.Core;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;


namespace Shinrai.Levels
{
    [Serializable]
    public class TileVisual
    {
        public TileBase Tile;
        public int Weight;
    }
    
    public class RoomRender : MonoBehaviour
    {
        [SerializeField] private int _roomWidth;
        [SerializeField] private int _roomHeight;
        [SerializeField] private Tilemap _groundTilemap;
        [SerializeField] private Tilemap _wallTilemap;
        [SerializeField] private TileBase[] _wallTiles;
        [SerializeField] private TileVisual[] _tileVisuals;

        [Header("Perlin Noise")]
        [SerializeField, Range(0f, 1f)] private float _specialTileCoverage = 0.3f;
        [SerializeField] private float _noiseScale = 0.12f;
        [SerializeField] private Vector2 _noiseOffset;

        [ContextMenu("Render Room")]
        public void Initialize()
        {
            var halfRoomWidth = _roomWidth / 2;
            var halfRoomHeight = _roomHeight / 2;
            _noiseOffset.x = Util.NextRandomFloat() * 10000f;
            _noiseOffset.y = Util.NextRandomFloat() * 10000f;

            for (int x = -halfRoomWidth; x < halfRoomWidth; x++)
            {
                for (int y = -halfRoomHeight; y < halfRoomHeight; y++)
                {
                    var sampleX = (x + halfRoomWidth + _noiseOffset.x) * _noiseScale;
                    var sampleY = (y + halfRoomHeight + _noiseOffset.y) * _noiseScale;

                    var noiseValue = Mathf.PerlinNoise(sampleX, sampleY);

                    int tileIndex = 0;

                    if (_tileVisuals.Length > 1 && noiseValue < _specialTileCoverage)
                    {
                        tileIndex = PickWeightedTileIndex(1, _tileVisuals.Length - 1);
                    }

                    _groundTilemap.SetTile(new Vector3Int(x, y, 0), _tileVisuals[tileIndex].Tile);
                }
            }

            DrawWall();
        }

        private void DrawWall()
        {
            var halfRoomWidth = (_roomWidth + 2) / 2;
            var halfRoomHeight = (_roomHeight + 2) / 2;

            for (int x = -halfRoomWidth; x < halfRoomWidth; x++)
            {
                for (int y = -halfRoomHeight; y < halfRoomHeight; y++)
                {
                    if (_wallTilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                    {
                        var tile = _wallTiles[Random.Range(0, _wallTiles.Length)];
                        _wallTilemap.SetTile(new Vector3Int(x, y, 0), tile);
                    }
                }
            }
        }
        

        private int PickWeightedTileIndex(int minIndex, int maxIndex)
        {
            int totalWeight = 0;
            for (int i = minIndex; i <= maxIndex; i++)
            {
                totalWeight += Mathf.Max(1, _tileVisuals[i].Weight);
            }

            int roll = UnityEngine.Random.Range(0, totalWeight);
            int current = 0;

            for (int i = minIndex; i <= maxIndex; i++)
            {
                current += Mathf.Max(1, _tileVisuals[i].Weight);
                if (roll < current)
                {
                    return i;
                }
            }

            return minIndex;
        }
    }
}
