using UnityEngine;
using NativeWebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance;
    private bool _isReady = false;

    private WebSocket websocket;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private async void Start()
    {
        //Utils.Log("Start NetworkManager");
        //await ConnectToServer("ws://paintingchess.duckdns.org:18000");
        //await ConnectToServer("wss://hachess.duckdns.org:18000");
        


        #if UNITY_WEBGL && !UNITY_EDITOR
        await ConnectToServer("/ws");
        #else
        await ConnectToServer("ws://localhost:8081/ws");
        #endif

    }

    public async Task ConnectToServer(string uri)
    {
        websocket = new WebSocket(uri);

        websocket.OnOpen += () =>
        {
            Utils.Log("WebSocket Connected");
            _isReady = true;
            Utils.Log("NetworkManager is now ready");
        };

        websocket.OnError += (e) =>
        {
            Utils.LogError("WebSocket Error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Utils.Log("WebSocket Closed");
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            Utils.Log("서버 응답: " + message);

            ResponsePacketData data = DeserializeResponse(message);
            print(data);

            if (_responseHandlers.TryGetValue(data.GetType(), out var handler))
            {
                handler(true, data);
            }
            else if (data is ResponsePacketData.Error error)
            {
                if (_responseSignalToType.TryGetValue(error.code, out var expectedType) &&
                    _responseHandlers.TryGetValue(expectedType, out var fallbackHandler))
                {
                    fallbackHandler(false, null!);
                }
                else
                {
                    throw new Exception("Unexpected Error Code");
                }
            }

            //OnServerMessageReceived?.Invoke(message);
        };

        await websocket.Connect();
    }

    public bool IsReady()
    {
        return _isReady;
    }

    // Callback
    //public event Action<string> OnServerMessageReceived;

    public async void SendMessageToServer(RequestPacketData data)
    {
#if SUPERSLOW
        await Task.Delay(3000);
#elif SLOW
        await Task.Delay(1000);
#endif

        string message = SerializeRequest(data);

        if (websocket != null && websocket.State == WebSocketState.Open)
        {
            await websocket.SendText(message);
            Utils.Log("Send Message To Server: " + message);
        }
        else
        {
            Utils.LogWarning("WebSocket is Closed.");
        }
    }



    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket?.DispatchMessageQueue();
#endif
    }

    private async void OnApplicationQuit()
    {
        if (websocket != null)
        {
            await websocket.Close();
        }
    }


    private static readonly Dictionary<Type, Action<bool, ResponsePacketData>> _responseHandlers =
    new()
    {

        {
            typeof(ResponsePacketData.Pong),
            (isSuccess, data) => {
                Utils.Log("Pong");
            }
        },
        {
            typeof(ResponsePacketData.EnterRoom),
            (isSuccess, data) => {
                Utils.Log("EnterRoom");
                FindObjectOfType<OutGameController>()?.OnResponseEnterRoom(isSuccess, (ResponsePacketData.EnterRoom)data);
            }
        },
        {
            typeof(ResponsePacketData.LeaveRoom),
            (isSuccess, data) => {
                Utils.Log("LeaveRoom");
                FindObjectOfType<OutGameController>()?.OnResponseLeaveRoom(isSuccess, (ResponsePacketData.LeaveRoom)data);
            }
        },
        {
            typeof(ResponsePacketData.GetRoomList),
            (isSuccess, data) => {
                Utils.Log("GetRoomList");
                FindObjectOfType<OutGameController>()?.OnResponseGetRoomList(isSuccess, (ResponsePacketData.GetRoomList)data);
            }
        },
        {
            typeof(ResponsePacketData.CreateRoom),
            (isSuccess, data) => {
                Utils.Log("CreateRoom");
                FindObjectOfType<OutGameController>()?.OnResponseCreateRoom(isSuccess, (ResponsePacketData.CreateRoom)data);
            }
        },
        {
            typeof(ResponsePacketData.PlayerCountChanged),
            (isSuccess, data) => {
                Utils.Log("PlayerCountChanged");
                FindObjectOfType<OutGameController>()?.OnResponsePlayerCountChanged(isSuccess, (ResponsePacketData.PlayerCountChanged)data);
            }
        },
        {
            typeof(ResponsePacketData.StartGame),
            (isSuccess, data) => {
                Utils.Log("StartGame");
                FindObjectOfType<OutGameController>()?.OnResponseStartGame(isSuccess, (ResponsePacketData.StartGame)data);
            }
        },
        {
            typeof(ResponsePacketData.ReadyGame),
            (isSuccess, data) => {
                Utils.Log("ReadyGame");
                FindObjectOfType<GameController>()?.OnResponseReadyGame(isSuccess, (ResponsePacketData.ReadyGame)data);
            }
        },
        {
            typeof(ResponsePacketData.OpenCard),
            (isSuccess, data) => {
                Utils.Log("OpenCard");
                FindObjectOfType<GameController>()?.OnResponseOpenCard(isSuccess, (ResponsePacketData.OpenCard)data);
            }
        },

        {
            typeof(ResponsePacketData.RingBellCorrect),
            (isSuccess, data) => {
                Utils.Log("RingBellCorrect");
                FindObjectOfType<GameController>()?.OnResponseRingBellCorrect(isSuccess, (ResponsePacketData.RingBellCorrect)data);
            }
        },

        {
            typeof(ResponsePacketData.RingBellWrong),
            (isSuccess, data) => {
                Utils.Log("RingBellWrong");
                FindObjectOfType<GameController>()?.OnResponseRingBellWrong(isSuccess, (ResponsePacketData.RingBellWrong)data);
            }
        },

        {
            typeof(ResponsePacketData.EndGame),
            (isSuccess, data) => {
                Utils.Log("EndGame");
                FindObjectOfType<GameController>()?.OnResponseEndGame(isSuccess, (ResponsePacketData.EndGame)data);
            }
        },
        {
            typeof(ResponsePacketData.Emotion),
            (isSuccess, data) => {
                Utils.Log("Emotion");
                FindObjectOfType<GameController>()?.OnResponseEmotion(isSuccess, (ResponsePacketData.Emotion)data);
            }
        },
        {
            typeof(ResponsePacketData.HowSlow),
            (isSuccess, data) => {
                Utils.Log("HowSlow");
                FindObjectOfType<GameController>()?.OnResponseHowSlow(isSuccess, (ResponsePacketData.HowSlow)data);
            }
        },
        {
            typeof(ResponsePacketData.CreateAccount),
            (isSuccess, data) => {
                Utils.Log("CreateAccount");
                FindObjectOfType<OutGameController>()?.OnResponseCreateAccount(isSuccess, (ResponsePacketData.CreateAccount)data);
            }
        },

        {
            typeof(ResponsePacketData.Login),
            (isSuccess, data) => {
                Utils.Log("Login");
                FindObjectOfType<OutGameController>()?.OnResponseLogin(isSuccess, (ResponsePacketData.Login)data);
            }
        },


        /*
        {
            typeof(ResponsePacketData.Login),
            (isSuccess, errorCode, data) => FindObjectOfType<SplashSceneController>()?.OnResponseLogin(isSuccess)
        },
        {
            typeof(ResponsePacketData.CreateRoom),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<OutGameController>()?.OnResponseCreateRoom(isSuccess, (ResponsePacketData.CreateRoom)data)
        },
        {
            typeof(ResponsePacketData.EnterRoom),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<OutGameController>()?.OnResponseEnterRoom(isSuccess, (ResponsePacketData.EnterRoom)data)
        },
        {
            typeof(ResponsePacketData.LeaveRoom),
            (isSuccess, errorCode, data) => {
                FindObjectOfType<OutGameController>()?.OnResponseLeaveRoom(isSuccess, (ResponsePacketData.LeaveRoom)data);
                FindObjectOfType<GameController>()?.OnResponseLeaveRoom(isSuccess, (ResponsePacketData.LeaveRoom)data);
            }
        },
        {
            typeof(ResponsePacketData.PlayerLeft),
            (isSuccess, errorCode, data) => {
                FindObjectOfType<OutGameController>()?.OnResponsePlayerLeft(isSuccess, (ResponsePacketData.PlayerLeft)data);
                FindObjectOfType<GameController>()?.OnResponsePlayerLeft(isSuccess, (ResponsePacketData.PlayerLeft)data);
            }
        },


        {
            typeof(ResponsePacketData.AcceptGame),
            (isSuccess, errorCode, data) => {
                FindObjectOfType<GameController>()?.OnResponseAcceptGame(isSuccess, (ResponsePacketData.AcceptGame)data);
                FindObjectOfType<OutGameController>()?.OnResponseAcceptGame(isSuccess, (ResponsePacketData.AcceptGame)data);
            }
        },

        {
            typeof(ResponsePacketData.StartGame),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseStartGame(isSuccess, (ResponsePacketData.StartGame)data)
        },

        {
            typeof(ResponsePacketData.EndGame),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseEndGame(isSuccess, (ResponsePacketData.EndGame)data)
        },

        
        {
            typeof(ResponsePacketData.Move),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseMove(isSuccess, (ResponsePacketData.Move)data)
        },
        {
            typeof(ResponsePacketData.Dive),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseDive(isSuccess, (ResponsePacketData.Dive)data)
        },
        {
            typeof(ResponsePacketData.Castling),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseCastle(isSuccess, (ResponsePacketData.Castling)data)
        },
        {
            typeof(ResponsePacketData.EnPassant),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseEnPassant(isSuccess, (ResponsePacketData.EnPassant)data)
        },
        {
            typeof(ResponsePacketData.Promotion),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponsePromotion(isSuccess, (ResponsePacketData.Promotion)data)
        },


        {
            typeof(ResponsePacketData.DrawOffer),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseDrawOffer(isSuccess, (ResponsePacketData.DrawOffer)data)
        },
        {
            typeof(ResponsePacketData.DrawCancel),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseDrawCancel(isSuccess, (ResponsePacketData.DrawCancel)data)
        },
        {
            typeof(ResponsePacketData.DrawResponse),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseDrawResponse(isSuccess, (ResponsePacketData.DrawResponse)data)
        },

        {
            typeof(ResponsePacketData.TackbackOffer),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseTackbackOffer(isSuccess, (ResponsePacketData.TackbackOffer)data)
        },
        {
            typeof(ResponsePacketData.TackbackCancel),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseTackbackCancel(isSuccess, (ResponsePacketData.TackbackCancel)data)
        },
        {
            typeof(ResponsePacketData.TackbackResponse),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseTackbackResponse(isSuccess, (ResponsePacketData.TackbackResponse)data)
        },

        {
            typeof(ResponsePacketData.Chat),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseChat(isSuccess, (ResponsePacketData.Chat)data)
        },

        {
            typeof(ResponsePacketData.DebuggingGameState),
            (isSuccess, errorCode, data) =>
                FindObjectOfType<GameController>()?.OnResponseDebuggingGameState(isSuccess, (ResponsePacketData.DebuggingGameState)data)
        }
        */
    };



    private static readonly Dictionary<int, Type> _requestSignalToType = new()
    {
        { 1, typeof(RequestPacketData.Ping) },
        { 1001, typeof(RequestPacketData.EnterRoom) },
        { 1002, typeof(RequestPacketData.LeaveRoom) },
        { 1003, typeof(RequestPacketData.GetRoomList) },
        { 1004, typeof(RequestPacketData.CreateRoom) },
        { 1011, typeof(RequestPacketData.ReadyGame) },
        { 2001, typeof(RequestPacketData.RingBell) },
        { 2004, typeof(RequestPacketData.Emotion) },
        { 4000, typeof(RequestPacketData.CreateAccount) },
        { 4001, typeof(RequestPacketData.Login) },
        /*
        { 1000, typeof(RequestPacketData.Login) },
        { 2000, typeof(RequestPacketData.CreateRoom) },
        { 2001, typeof(RequestPacketData.EnterRoom) },
        { 2002, typeof(RequestPacketData.LeaveRoom) },
        { 3000, typeof(RequestPacketData.AcceptGame) },
        { 3001, typeof(RequestPacketData.GameReady) },

        { 3004, typeof(RequestPacketData.Rematch) },
        { 3005, typeof(RequestPacketData.RematchCancel) },


        { 3010, typeof(RequestPacketData.Move) },
        { 3011, typeof(RequestPacketData.Dive) },
        { 3012, typeof(RequestPacketData.Castling) },
        { 3013, typeof(RequestPacketData.EnPassant) },
        { 3014, typeof(RequestPacketData.Promotion) },

        { 3020, typeof(RequestPacketData.Resign) },
        { 3021, typeof(RequestPacketData.DrawOffer) },
        { 3022, typeof(RequestPacketData.DrawCancel) },
        { 3023, typeof(RequestPacketData.DrawResponse) },
        { 3024, typeof(RequestPacketData.TackbackOffer) },
        { 3025, typeof(RequestPacketData.TackbackCancel) },
        { 3026, typeof(RequestPacketData.TackbackResponse) },

        { 4000, typeof(RequestPacketData.Chat) },

        { 9000, typeof(RequestPacketData.DebuggingGameState) },
        */
    };

    private static readonly Dictionary<int, Type> _responseSignalToType = new()
    {
        { 1, typeof(ResponsePacketData.Pong) },
        { 1001, typeof(ResponsePacketData.EnterRoom) },
        { 1002, typeof(ResponsePacketData.LeaveRoom) },
        { 1003, typeof(ResponsePacketData.GetRoomList) },
        { 1004, typeof(ResponsePacketData.CreateRoom) },
        { 1005, typeof(ResponsePacketData.PlayerCountChanged) },
        { 1010, typeof(ResponsePacketData.StartGame) },
        { 1011, typeof(ResponsePacketData.ReadyGame) },
        { 2000, typeof(ResponsePacketData.OpenCard) },
        { 2002, typeof(ResponsePacketData.RingBellCorrect) },
        { 2003, typeof(ResponsePacketData.RingBellWrong) },
        { 2004, typeof(ResponsePacketData.Emotion) },
        { 2005, typeof(ResponsePacketData.HowSlow) },
        { 3000, typeof(ResponsePacketData.EndGame) },

        { 4000, typeof(ResponsePacketData.CreateAccount) },
        { 4001, typeof(ResponsePacketData.Login) },

        /*
        { 1000, typeof(ResponsePacketData.Login) },
        { 2000, typeof(ResponsePacketData.CreateRoom) },
        { 2001, typeof(ResponsePacketData.EnterRoom) },
        { 2002, typeof(ResponsePacketData.LeaveRoom) },
        { 2003, typeof(ResponsePacketData.PlayerLeft) },
        
        { 3000, typeof(ResponsePacketData.AcceptGame) },
        { 3002, typeof(ResponsePacketData.StartGame) },
        { 3003, typeof(ResponsePacketData.EndGame) },

        { 3010, typeof(ResponsePacketData.Move) },
        { 3011, typeof(ResponsePacketData.Dive) },
        { 3012, typeof(ResponsePacketData.Castling) },
        { 3013, typeof(ResponsePacketData.EnPassant) },
        { 3014, typeof(ResponsePacketData.Promotion) },

        { 3021, typeof(ResponsePacketData.DrawOffer) },
        { 3022, typeof(ResponsePacketData.DrawCancel) },
        { 3023, typeof(ResponsePacketData.DrawResponse) },
        { 3024, typeof(ResponsePacketData.TackbackOffer) },
        { 3025, typeof(ResponsePacketData.TackbackCancel) },
        { 3026, typeof(ResponsePacketData.TackbackResponse) },

        { 4000, typeof(ResponsePacketData.Chat) },

        { 9000, typeof(ResponsePacketData.DebuggingGameState) },
        */
    };

    public static Type GetRequestTypeFromSignal(int signal)
    {
        if (_requestSignalToType.TryGetValue(signal, out var type))
            return type;

        throw new ArgumentException($"Unknown request signal code: {signal}");
    }

    public static int GetRequestSignalFromType(Type type)
    {
        foreach (var kv in _requestSignalToType)
        {
            if (kv.Value == type)
                return kv.Key;
        }

        throw new ArgumentException($"Unknown request type: {type.Name}");
    }

    public static Type GetResponseTypeFromSignal(int signal)
    {
        if (_responseSignalToType.TryGetValue(signal, out var type))
            return type;

        throw new ArgumentException($"Unknown request signal code: {signal}");
    }

    public static int GetResponseSignalFromType(Type type)
    {
        foreach (var kv in _responseSignalToType)
        {
            if (kv.Value == type)
                return kv.Key;
        }

        throw new ArgumentException($"Unknown request type: {type.Name}");
    }


    public static RequestPacketData DeserializeRequest(string json)
    {
        var jo = JObject.Parse(json);

        int signal = jo["signal"]?.Value<int>()
            ?? throw new JsonSerializationException("Missing 'signal' field");

        var dataToken = jo["data"];
        if (dataToken == null)
            throw new JsonSerializationException("Missing 'data' field");

        var targetType = GetRequestTypeFromSignal(signal);
        var dataObj = (RequestPacketData)dataToken.ToObject(targetType)!;

        return dataObj;
    }

    public static string SerializeRequest(RequestPacketData data)
    {
        var obj = new
        {
            signal = GetRequestSignalFromType(data.GetType()),
            data = data
        };

        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }

    public bool IsSuccess(string json)
    {
        var jo = JObject.Parse(json);
        var code = jo["code"]?.Value<int>();
        return (code != null) && (code == 200);
    }
    
    public static ResponsePacketData DeserializeResponse(string json)
    {
        var jo = JObject.Parse(json);

        int signal = jo["signal"]?.Value<int>()
            ?? throw new JsonSerializationException("Missing 'signal' field");

        var dataToken = jo["data"];
        if (dataToken == null)
            throw new JsonSerializationException("Missing 'data' field");

        var code = jo["code"]?.Value<int>()
            ?? throw new JsonSerializationException("Missing 'code' field");

        //var errorCode = jo["errorCode"]?.Value<int>()
        //    ?? throw new JsonSerializationException("Missing 'errorCode' field");

        if (code != 200)
        {
            //return new ResponsePacketData.Error(signal, errorCode);
            return new ResponsePacketData.Error(signal);
        }

        var targetType = GetResponseTypeFromSignal(signal);
        var dataObj = (ResponsePacketData)dataToken.ToObject(targetType)!;

        return dataObj;
    }
    
    public static string SerializeResponse(ResponsePacketData data)
    {
        var obj = new
        {
            signal = GetResponseSignalFromType(data.GetType()),
            data = data
        };

        return JsonConvert.SerializeObject(obj, Formatting.Indented);
    }
}



public record RequestPacket(int signal, RequestPacketData data);
public record ResponsePacket(int signal, ResponsePacketData data);

public abstract record RequestPacketData
{
    public sealed record Ping() : RequestPacketData;
    public sealed record EnterRoom(int roomId) : RequestPacketData;
    public sealed record LeaveRoom() : RequestPacketData;
    public sealed record GetRoomList() : RequestPacketData;
    public sealed record CreateRoom(string roomName, int maxPlayerCount, int fruitVariation, int fruitCount, int speed) : RequestPacketData;
    public sealed record ReadyGame() : RequestPacketData;

    

    public sealed record RingBell() : RequestPacketData;

    public sealed record Emotion(int emotionType) : RequestPacketData;


    public sealed record CreateAccount(string id, string password, string nickname) : RequestPacketData; // 4000
    public sealed record Login(string id, string password) : RequestPacketData; // 4001
    
}

public abstract record ResponsePacketData
{
    public sealed record Error(int code) : ResponsePacketData;
    public sealed record Pong() : ResponsePacketData;
    public sealed record EnterRoom(int roomId, string roomName, int maxPlayers, int fruitVariation, int fruitBellCount, int gameTempo) : ResponsePacketData;
    public sealed record LeaveRoom() : ResponsePacketData;
    public sealed record CreateRoom(int roomID) : ResponsePacketData;

    public sealed record GetRoomList(RoomInfo[] rooms) : ResponsePacketData;
    public sealed record PlayerCountChanged(int playerCount) : ResponsePacketData;
    
    
    public sealed record StartGame(int playerCount, string[] playerNames, int myIndex, int startingCards, int gameTimeLimit, int fruitVariation, int fruitBellCount, int gameTempo) : ResponsePacketData;
    public sealed record ReadyGame() : ResponsePacketData;
    public sealed record OpenCard(int fruitIndex, int fruitCount, int playerIndex) : ResponsePacketData;
    public sealed record RingBellCorrect(int playerIndex, int[] playerCards) : ResponsePacketData;
    public sealed record RingBellWrong(int playerIndex, bool[] cardGivenTo, int[] playerCards) : ResponsePacketData;

    public sealed record Emotion(int playerIndex, int emotionType) : ResponsePacketData;
    public sealed record HowSlow(int delayMs) : ResponsePacketData;

    public sealed record EndGame(int[] playerCards, int[] playerRanks) : ResponsePacketData;

    public sealed record CreateAccount(string id) : ResponsePacketData;

    public sealed record Login(string id, string nickname) : ResponsePacketData;

}





/*

public record RequestPacket(int signal, RequestPacketData data);
public record ResponsePacket(int signal, ResponsePacketData data);

public abstract record RequestPacketData
{
    public sealed record Login() : RequestPacketData;
    public sealed record CreateRoom(string side, int initialTime, int incrementTime) : RequestPacketData;
    public sealed record EnterRoom(string roomcode) : RequestPacketData;
    public sealed record LeaveRoom() : RequestPacketData;
    public sealed record AcceptGame() : RequestPacketData;
    public sealed record GameReady() : RequestPacketData;

    public sealed record Rematch() : RequestPacketData;
    public sealed record RematchCancel() : RequestPacketData;

    public sealed record Move(int[] startPos, int[] endPos) : RequestPacketData;
    public sealed record Dive(int[] startPos, int[] endPos) : RequestPacketData;
    public sealed record Castling(int[] startPos, int[] endPos, int[] castlingRookPos) : RequestPacketData;
    public sealed record EnPassant(int[] startPos, int[] endPos, int[] enPassantTargetPos) : RequestPacketData;
    public sealed record Promotion(int[] startPos, int[] endPos, string promotionType) : RequestPacketData;


    public sealed record Resign() : RequestPacketData;
    public sealed record DrawOffer() : RequestPacketData;
    public sealed record DrawCancel() : RequestPacketData;
    public sealed record DrawResponse(bool accept) : RequestPacketData;

    public sealed record TackbackOffer() : RequestPacketData;
    public sealed record TackbackCancel() : RequestPacketData;
    public sealed record TackbackResponse(bool accept) : RequestPacketData;

    public sealed record Chat(string message) : RequestPacketData;

    public sealed record DebuggingGameState() : RequestPacketData;

}

public abstract record ResponsePacketData
{
    public sealed record Error(int code, int errorCode) : ResponsePacketData;
    public sealed record Login() : ResponsePacketData;
    public sealed record CreateRoom(string roomcode) : ResponsePacketData;
    public sealed record EnterRoom(string roomcode) : ResponsePacketData;
    public sealed record LeaveRoom() : ResponsePacketData;
    public sealed record PlayerLeft() : ResponsePacketData;
    public sealed record AcceptGame(string side, int initialTime) : ResponsePacketData;
    public sealed record StartGame() : ResponsePacketData;
    public sealed record EndGame(string result, string reason) : ResponsePacketData;

    public sealed record Move(string whoseMove, int[] startPos, int[] endPos, RemainTime remainTime) : ResponsePacketData;
    public sealed record Dive(string whoseMove, int[] startPos, int[] endPos, RemainTime remainTime) : ResponsePacketData;
    public sealed record Castling(string whoseMove, int[] startPos, int[] endPos, int[] castlingRookPos, RemainTime remainTime) : ResponsePacketData;
    public sealed record EnPassant(string whoseMove, int[] startPos, int[] endPos, int[] enPassantTargetPos, RemainTime remainTime) : ResponsePacketData;
    public sealed record Promotion(string whoseMove, int[] startPos, int[] endPos, string promotionType, RemainTime remainTime) : ResponsePacketData;


    public sealed record DrawOffer() : ResponsePacketData;
    public sealed record DrawCancel() : ResponsePacketData;
    public sealed record DrawResponse(bool accept) : ResponsePacketData;

    public sealed record TackbackOffer() : ResponsePacketData;
    public sealed record TackbackCancel() : ResponsePacketData;
    public sealed record TackbackResponse(bool accept) : ResponsePacketData;


    public sealed record Chat(string whoseMessage, string message) : ResponsePacketData;

    public sealed record DebuggingGameState(string turn, string[,] pieces, string[,] colors, CastlingInfo canRedCastle, CastlingInfo canBlueCastle, int[] enPassantPos, RemainTime remainTime) : ResponsePacketData;

}


public sealed record RemainTime(int red, int blue);
public sealed record CastlingInfo(bool king, bool queen);

*/

public sealed record RoomInfo(int roomID, string roomName, int playerCount, int maxPlayerCount, int fruitVariation, int fruitCount, int speed);
