﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TR.BIDSSMemLib
{
  public partial class SMemLib
  {
    //structをまとめ
    /// <summary>車両のスペック(互換性確保)</summary>
    public struct Spec
    {
      /// <summary>ブレーキ段数</summary>
      public int B;
      /// <summary>ノッチ段数</summary>
      public int P;
      /// <summary>ATS確認段数</summary>
      public int A;
      /// <summary>常用最大段数</summary>
      public int J;
      /// <summary>編成車両数</summary>
      public int C;
    };
    /// <summary>車両の状態(互換性確保)</summary>
    public struct State
    {
      /// <summary>列車位置[m]</summary>
      public double Z;
      /// <summary>列車速度[km/h]</summary>
      public float V;
      /// <summary>0時からの経過時間[ms]</summary>
      public int T;
      /// <summary>BC圧力[kPa]</summary>
      public float BC;
      /// <summary>MR圧力[kPa]</summary>
      public float MR;
      /// <summary>ER圧力[kPa]</summary>
      public float ER;
      /// <summary>BP圧力[kPa]</summary>
      public float BP;
      /// <summary>SAP圧力[kPa]</summary>
      public float SAP;
      /// <summary>電流[A]</summary>
      public float I;
    };
    /// <summary>車両のハンドル位置(互換性確保)</summary>
    public struct Hand
    {
      /// <summary>ブレーキハンドル位置</summary>
      public int B;
      /// <summary>ノッチハンドル位置</summary>
      public int P;
      /// <summary>レバーサーハンドル位置</summary>
      public int R;
      /// <summary>定速制御状態</summary>
      public int C;
    };
    /// <summary>BIDSSharedMemoryのデータ構造体(互換性確保)</summary>
    public struct BIDSSharedMemoryData
    {
      /// <summary>SharedMemoryが有効かどうか</summary>
      public bool IsEnabled;
      /// <summary>SharedRAMの構造バージョン</summary>
      public int VersionNum;
      /// <summary>車両スペック情報</summary>
      public Spec SpecData;
      /// <summary>車両状態情報</summary>
      public State StateData;
      /// <summary>ハンドル位置情報</summary>
      public Hand HandleData;
      /// <summary>ドアが閉まっているかどうか</summary>
      public bool IsDoorClosed;
      /// <summary>Panelの表示番号配列</summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
      public int[] Panel;
      /// <summary>Soundの値配列</summary>
      [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
      public int[] Sound;
    };

    static private readonly uint BSMDsize = (uint)Marshal.SizeOf(typeof(BIDSSharedMemoryData));

    /// <summary>毎フレームごとに取得できるデータ(本家/open共通)</summary>
    public struct ElapD
    {
      /// <summary>情報が有効かどうか</summary>
      public bool IsEnabled { get; set; }
      /// <summary>クラスバージョン</summary>
      public readonly int Ver;
      /// <summary>列車位置[m]</summary>
      public double Location { get; set; }
      /// <summary>列車速度[km/h]</summary>
      public double Speed { get; set; }
      /// <summary>BC圧力[kPa]</summary>
      public double BCPres { get; set; }
      /// <summary>MR圧力[kPa]</summary>
      public double MRPres { get; set; }
      /// <summary>ER圧力[kPa]</summary>
      public double ERPres { get; set; }
      /// <summary>BP圧力[kPa]</summary>
      public double BPPres { get; set; }
      /// <summary>SAP圧力[kPa]</summary>
      public double SAPPres { get; set; }
      /// <summary>電流[A]</summary>
      public double Current { get; set; }
      /// <summary>(未実装)架線電圧[V]</summary>
      public double Voltage { get; set; }
      /// <summary>0時からの経過時間[ms]</summary>
      public int Time { get; set; }
      /// <summary>現在時刻[時]</summary>
      public byte TimeHH { get { return (byte)TimeSpan.FromMilliseconds(Time).Hours; } }
      /// <summary>現在時刻[分]</summary>
      public byte TimeMM { get { return (byte)TimeSpan.FromMilliseconds(Time).Minutes; } }
      /// <summary>現在時刻[秒]</summary>
      public byte TimeSS { get { return (byte)TimeSpan.FromMilliseconds(Time).Seconds; } }
      /// <summary>現在時刻[ミリ秒]</summary>
      public short TimeMS { get { return (short)TimeSpan.FromMilliseconds(Time).Milliseconds; } }
      public uint DoorState { get; set; }
    }

    /// <summary>毎フレームごとに取得できるデータ(open専用)</summary>
    public struct OElapD
    {
      /// <summary>情報が有効かどうか</summary>
      public bool IsEnabled { get; set; }
      /// <summary>クラスバージョン</summary>
      public readonly int Ver;
      /// <summary>現在のカーブ半径[m]</summary>
      public double Radius { get; set; }
      /// <summary>現在のカントの大きさ[mm]</summary>
      public double Cant { get; set; }
      /// <summary>現在の軌間[mm]</summary>
      public double Pitch { get; set; }
      /// <summary>1フレーム当たりの時間[ms]</summary>
      public ushort ElapTime { get; set; }
      /// <summary>先行列車に関する情報</summary>
      public PreTrainD PreTrain { get; set; }

      public struct PreTrainD
      {
        /// <summary>情報が有効かどうか</summary>
        public bool IsEnabled { get; set; }
        /// <summary>先行列車の尻尾の位置[m]</summary>
        public double Location { get; set; }
        /// <summary>先行列車の尻尾までの距離[m]</summary>
        public double Distance { get; set; }
        /// <summary>先行列車の速度[km/h]</summary>
        public double Speed { get; set; }
      }
    }

    /// <summary>ハンドルに関する情報</summary>
    public struct HandleD
    {
      /// <summary>クラスバージョン</summary>
      public readonly int Ver;
      /// <summary>情報が有効かどうか</summary>
      public bool IsEnabled { get; set; }
      /// <summary>レバーサー位置</summary>
      public sbyte Reverser { get; set; }
      /// <summary>Pノッチ位置</summary>
      public short PNotch { get; set; }
      /// <summary>Bノッチ位置</summary>
      public ushort BNotch { get; set; }
      /// <summary>単弁位置</summary>
      public ushort LocoBNotch { get; set; }
      /// <summary>定速制御の有効状態</summary>
      public bool ConstSP { get; set; }
    }

    /// <summary>駅に関するデータ</summary>
    public struct StaD
    {
      /// <summary>データサイズ</summary>
      public int Size => Marshal.SizeOf(StaList);
      public List<StationData> StaList { get; set; }
      /// <summary>駅に関するデータ</summary>
      public struct StationData
      {
        /// <summary>駅名番号</summary>
        public byte StaName;
        /// <summary>到着予定時刻[s]の1/5</summary>
        public ushort ArrTime;
        /// <summary>発車予定時刻[s]の1/5</summary>
        public ushort DepTime;
        /// <summary>標準停車時間[s]の1/5</summary>
        public ushort StopTime;
        /// <summary>停止定位駅かどうか</summary>
        public bool IsSigUsuallyRed;
        /// <summary>左のドアが開くかどうか</summary>
        public bool IsLeftOpen;
        /// <summary>右のドアが開くかどうか</summary>
        public bool IsRightOpen;
        /// <summary>予定された発着番線</summary>
        public float DefaultTrack;
        /// <summary>停止位置[m]</summary>
        public double StopLocation;
        /// <summary>通過駅かどうか</summary>
        public bool IsPass;
        /// <summary>駅の種類</summary>
        public StaType Type;

        /// <summary>駅の種類</summary>
        public enum StaType { Normal, ChangeEnd, Termial, Stop }
      }
    }

    public struct PanelD
    {
      public int Size => Marshal.SizeOf(0);
      public int[] Panels { get; set; }
    }
    public struct SoundD
    {
      public int Size => Marshal.SizeOf(0);
      public int[] Sounds { get; set; }
    }
  }
}