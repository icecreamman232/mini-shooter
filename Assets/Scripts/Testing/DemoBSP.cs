
using System;
using System.Collections.Generic;
using Shinrai.Core;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[Serializable]
public class DungeonData
{
    public List<RectInt> Rooms = new();
    public List<Vector2Int[]> Corridors = new(); // Mỗi corridor là 2 điểm: [start, end]
}


public class DemoBSP : MonoBehaviour
{
    public int MapWidth = 50;
    public int MapHeight = 50;
    public float RoomMinRatio = 0.5f;
    public float RoomMaxRatio = 0.75f;
    public int RoomPadding = 2;
    public Tilemap GroundTilemap;
    public Tilemap WallTilemap;
    public TileBase[] GroundTile;
    public TileBase[] WallTile;
    private DungeonData _dungeonData;
    private BinarySpacePartionTree _tree;
    
    private void Start()
    {
        _dungeonData = new DungeonData();
        _tree = new BinarySpacePartionTree(0, 0, MapWidth, MapHeight);
        PlaceRoom(_tree.Root);
        ConnectRooms(_tree.Root);
        DrawRoomTiles();
    }

    private void PlaceRoom(BSPNode node)
    {
        if (!node.IsLeaf)
        {
            if(node.Left != null) PlaceRoom(node.Left);
            if(node.Right != null) PlaceRoom(node.Right);
            return;
        }
        
        int roomW = Random.Range(Mathf.RoundToInt(node.Bounds.width * RoomMinRatio),
                        Mathf.RoundToInt(node.Bounds.width * RoomMaxRatio));
            
        int roomH = Random.Range(Mathf.RoundToInt(node.Bounds.height * RoomMinRatio),
                        Mathf.RoundToInt(node.Bounds.height * RoomMaxRatio));
        
        int roomX = Random.Range(node.Bounds.x + RoomPadding, node.Bounds.xMax - roomW - RoomPadding);
        int roomY = Random.Range(node.Bounds.y + RoomPadding, node.Bounds.yMax - roomH - RoomPadding);
        
        node.Room = new RectInt(roomX, roomY, roomW, roomH);
        _dungeonData.Rooms.Add(node.Room.Value);
    }
    
    private void ConnectRooms(BSPNode node)
    {
        if (node.IsLeaf) return;

        if (node.Left  != null) ConnectRooms(node.Left);
        if (node.Right != null) ConnectRooms(node.Right);

        // Lấy tất cả rooms từ mỗi subtree
        var roomsA = GetAllRooms(node.Left);
        var roomsB = GetAllRooms(node.Right);

        if (roomsA.Count == 0 || roomsB.Count == 0) return;

        // Tìm cặp room gần nhau nhất giữa 2 subtree
        RectInt bestA = roomsA[0], bestB = roomsB[0];
        float bestDist = float.MaxValue;

        foreach (var a in roomsA)
        {
            var centerA = new Vector2Int(a.x + a.width / 2, a.y + a.height / 2);
            foreach (var b in roomsB)
            {
                var centerB = new Vector2Int(b.x + b.width / 2, b.y + b.height / 2);
                float dist = Vector2Int.Distance(centerA, centerB);
                if (dist < bestDist)
                {
                    bestDist = dist;
                    bestA = a;
                    bestB = b;
                }
            }
        }

        var cA = new Vector2Int(bestA.x + bestA.width / 2, bestA.y + bestA.height / 2);
        var cB = new Vector2Int(bestB.x + bestB.width / 2, bestB.y + bestB.height / 2);
        _dungeonData.Corridors.Add(new[] { cA, cB });
    }

    List<RectInt> GetAllRooms(BSPNode node)
    {
        var result = new List<RectInt>();
        if (node == null) return result;
        if (node.Room.HasValue)
        {
            result.Add(node.Room.Value);
            return result;
        }
        result.AddRange(GetAllRooms(node.Left));
        result.AddRange(GetAllRooms(node.Right));
        return result;
    }

    private void DrawRoomTiles()
    {
        foreach (var room in _dungeonData.Rooms)
        {
            for (int x = room.x; x < room.x + room.width; x++)
            {
                for (int y = room.y; y < room.y + room.height; y++)
                {
                    if (x == room.x || y == room.y || x == room.x + room.width - 1 || y == room.y + room.height - 1)
                    {
                        WallTilemap.SetTile(new Vector3Int(x, y, 0), WallTile.PickRandom());
                    }
                    else
                    {
                        GroundTilemap.SetTile(new Vector3Int(x, y, 0), GroundTile.PickRandom());
                    }
                }
            }
        }
    }
    
    #region Gizmos
    
    private void OnDrawGizmosSelected()
    {
        if (_tree == null) return;
        _tree.Draw();
        DrawRoomsGizmos();
        DrawCorridorsGizmos();
    }
    
    private void DrawRoomsGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.8f);
        foreach (var room in _dungeonData.Rooms)
        {
            Gizmos.DrawCube(
                new Vector3(room.x + room.width * 0.5f, room.y + room.height * 0.5f, 0),
                new Vector3(room.width, room.height, 0)
            );
        }
    }
    
    private void DrawCorridorsGizmos()
    {
        Gizmos.color = new Color(1f, 0.6f, 0.1f, 0.9f);
        foreach (var corridor in _dungeonData.Corridors)
        {
            var a = new Vector3(corridor[0].x, corridor[0].y, 0);
            var b = new Vector3(corridor[1].x, corridor[0].y, 0); // bend point
            var c = new Vector3(corridor[1].x, corridor[1].y, 0);
            Gizmos.DrawLine(a, b);
            Gizmos.DrawLine(b, c);
        }
    }
    
    #endregion
}
