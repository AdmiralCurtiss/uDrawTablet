﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace uDrawTablet
{
  public class TabletSettings
  {
    #region Declarations

    private const string _DEFAULT_SECTION = "Default";
    private const string _KEY_PEN_THRESHOLD = "PenThreshold";
    private const int _DEFAULT_PEN_THRESHOLD = 5;
    private const string _KEY_MOVEMENT_TYPE = "MovementType";
    private const string _KEY_MOVEMENT_SPEED = "MovementSpeed";
    private const int _DEFAULT_MOVEMENT_SPEED = 10;
    private const string _KEY_PRECISION = "Precision";
    private const int _DEFAULT_PRECISION = 3;
    private const string _KEY_A_ACTION = "AAction";
    private const string _DEFAULT_A_ACTION = "LeftClick";
    private const string _KEY_B_ACTION = "BAction";
    private const string _DEFAULT_B_ACTION = "None";
    private const string _KEY_X_ACTION = "XAction";
    private const string _DEFAULT_X_ACTION = "RightClick";
    private const string _KEY_Y_ACTION = "YAction";
    private const string _DEFAULT_Y_ACTION = "ShowOptions";
    private const string _KEY_UP_ACTION = "UpAction";
    private const string _DEFAULT_UP_ACTION = "MoveUp";
    private const string _KEY_DOWN_ACTION = "DownAction";
    private const string _DEFAULT_DOWN_ACTION = "MoveDown";
    private const string _KEY_LEFT_ACTION = "LeftAction";
    private const string _DEFAULT_LEFT_ACTION = "MoveLeft";
    private const string _KEY_RIGHT_ACTION = "RightAction";
    private const string _DEFAULT_RIGHT_ACTION = "MoveRight";
    private const string _KEY_START_ACTION = "StartAction";
    private const string _DEFAULT_START_ACTION = "None";
    private const string _KEY_BACK_ACTION = "BackAction";
    private const string _DEFAULT_BACK_ACTION = "None";
    private const string _KEY_GUIDE_ACTION = "GuideAction";
    private const string _DEFAULT_GUIDE_ACTION = "TurnOffTablet";

    public enum TabletMovementType
    {
      Absolute,
      Relative
    };

    public int PenPressureThreshold { get; set; }
    public TabletMovementType MovementType { get; set; }
    public int MovementSpeed { get; set; }
    public int Precision { get; set; }
    public TabletOptionButton.ButtonAction AAction { get; set; }
    public TabletOptionButton.ButtonAction BAction { get; set; }
    public TabletOptionButton.ButtonAction XAction { get; set; }
    public TabletOptionButton.ButtonAction YAction { get; set; }
    public TabletOptionButton.ButtonAction UpAction { get; set; }
    public TabletOptionButton.ButtonAction DownAction { get; set; }
    public TabletOptionButton.ButtonAction LeftAction { get; set; }
    public TabletOptionButton.ButtonAction RightAction { get; set; }
    public TabletOptionButton.ButtonAction StartAction { get; set; }
    public TabletOptionButton.ButtonAction BackAction { get; set; }
    public TabletOptionButton.ButtonAction GuideAction { get; set; }

    #endregion

    #region P/Invoke Crud

    [DllImport("kernel32")]
    private static extern long WritePrivateProfileString(string lPAppName, string lpKeyName, string lpString, string lpFileName);

    [DllImport("kernel32")]
    private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFilePath);

    #endregion

    #region Constructors/Teardown

    public TabletSettings()
    {
      PenPressureThreshold = _DEFAULT_PEN_THRESHOLD;
      MovementType = TabletMovementType.Absolute;
      MovementSpeed = _DEFAULT_MOVEMENT_SPEED;
      Precision = _DEFAULT_PRECISION;
      AAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_A_ACTION);
      BAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_B_ACTION);
      XAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_X_ACTION);
      YAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_Y_ACTION);
      UpAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_UP_ACTION);
      DownAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_DOWN_ACTION);
      LeftAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_LEFT_ACTION);
      RightAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_RIGHT_ACTION);
      StartAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_START_ACTION);
      BackAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_BACK_ACTION);
      GuideAction = (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), _DEFAULT_GUIDE_ACTION);
    }

    #endregion

    #region Public Methods

    public static TabletSettings LoadSettings(string iniFileName)
    {
      var ret = new TabletSettings();

      //Pen pressure threshold
      var sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_PEN_THRESHOLD, ret.PenPressureThreshold.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      int threshold = ret.PenPressureThreshold; int.TryParse(sb.ToString(), out threshold);
      ret.PenPressureThreshold = threshold;

      //Movement type
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_MOVEMENT_TYPE, ret.MovementType.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      var type = ret.MovementType; type = (TabletMovementType)Enum.Parse(typeof(TabletMovementType), sb.ToString());
      ret.MovementType = type;

      //Movement speed
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_MOVEMENT_SPEED, ret.MovementSpeed.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      int speed = ret.MovementSpeed; int.TryParse(sb.ToString(), out speed);
      ret.MovementSpeed = speed;

      //Movement precision
      sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, _KEY_PRECISION, ret.Precision.ToString(), sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
      int precision = ret.Precision; int.TryParse(sb.ToString(), out precision);
      ret.Precision = precision;

      //Actions
      ret.AAction = _GetAction(iniFileName, _KEY_A_ACTION, _DEFAULT_A_ACTION);
      ret.BAction = _GetAction(iniFileName, _KEY_B_ACTION, _DEFAULT_B_ACTION);
      ret.XAction = _GetAction(iniFileName, _KEY_X_ACTION, _DEFAULT_X_ACTION);
      ret.YAction = _GetAction(iniFileName, _KEY_Y_ACTION, _DEFAULT_Y_ACTION);
      ret.UpAction = _GetAction(iniFileName, _KEY_UP_ACTION, _DEFAULT_UP_ACTION);
      ret.DownAction = _GetAction(iniFileName, _KEY_DOWN_ACTION, _DEFAULT_DOWN_ACTION);
      ret.LeftAction = _GetAction(iniFileName, _KEY_LEFT_ACTION, _DEFAULT_LEFT_ACTION);
      ret.RightAction = _GetAction(iniFileName, _KEY_RIGHT_ACTION, _DEFAULT_RIGHT_ACTION);
      ret.StartAction = _GetAction(iniFileName, _KEY_START_ACTION, _DEFAULT_START_ACTION);
      ret.BackAction = _GetAction(iniFileName, _KEY_BACK_ACTION, _DEFAULT_BACK_ACTION);
      ret.GuideAction = _GetAction(iniFileName, _KEY_GUIDE_ACTION, _DEFAULT_GUIDE_ACTION);

      return ret;
    }

    public void SaveSettings(string iniFileName)
    {
      //Pen pressure threshold
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_PEN_THRESHOLD, this.PenPressureThreshold.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Movement type
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_MOVEMENT_TYPE, this.MovementType.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Movement speed
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_MOVEMENT_SPEED, this.MovementSpeed.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Movement precision
      WritePrivateProfileString(_DEFAULT_SECTION, _KEY_PRECISION, this.Precision.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      //Actions
      _SetAction(iniFileName, _KEY_A_ACTION, AAction);
      _SetAction(iniFileName, _KEY_B_ACTION, BAction);
      _SetAction(iniFileName, _KEY_X_ACTION, XAction);
      _SetAction(iniFileName, _KEY_Y_ACTION, YAction);
      _SetAction(iniFileName, _KEY_UP_ACTION, UpAction);
      _SetAction(iniFileName, _KEY_DOWN_ACTION, DownAction);
      _SetAction(iniFileName, _KEY_LEFT_ACTION, LeftAction);
      _SetAction(iniFileName, _KEY_RIGHT_ACTION, RightAction);
      _SetAction(iniFileName, _KEY_START_ACTION, StartAction);
      _SetAction(iniFileName, _KEY_BACK_ACTION, BackAction);
      _SetAction(iniFileName, _KEY_GUIDE_ACTION, GuideAction);
    }

    #endregion

    #region Local Methods

    private static TabletOptionButton.ButtonAction _GetAction(string iniFileName, string keyName, string defaultValue)
    {
      var sb = new StringBuilder(255);
      GetPrivateProfileString(_DEFAULT_SECTION, keyName, defaultValue, sb, sb.Capacity,
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));

      return (TabletOptionButton.ButtonAction)Enum.Parse(typeof(TabletOptionButton.ButtonAction), sb.ToString());
    }

    private static void _SetAction(string iniFileName, string keyName, TabletOptionButton.ButtonAction action)
    {
      WritePrivateProfileString(_DEFAULT_SECTION, keyName, action.ToString(),
        Path.Combine(Directory.GetCurrentDirectory(), iniFileName));
    }

    #endregion
  }
}
