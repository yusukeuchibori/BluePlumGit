﻿
namespace GitlabTool.ViewModels
{
    using Common.Library.Entitys;
    using Common.Library.Enums;
    using GitlabTool;
    using GitlabTool.Models;
    using Gordias.Library.Headquarters;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    #region メインクラス
    /// <summary>
    /// メインウインドウビューモデル
    /// </summary>
    public class MainWindowViewModel : TacticsViewModel<MainWindowViewModelProperty, MainWindowViewModelCommand>
	{
        /// <summary>
        /// モデル
        /// </summary>
        private MainWindowModel model;

        /// <summary>
        /// グローバルコンフィグ
        /// </summary>
        private GrobalConfigEntity globalConfig;

        /// <summary>
        /// アプリケーション設定
        /// </summary>
        private ConfigEntity config;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            this.model = new MainWindowModel();
        }


		#region WindowState 変更通知プロパティ

		private WindowState _WindowState;

		public WindowState WindowState
		{
			get { return this._WindowState; }
			set
			{
				if (this._WindowState != value)
				{
					this._WindowState = value;
                    this.IsMaximized = value == WindowState.Maximized;
                    this.CanNormalize = value == WindowState.Maximized;
                    this.CanMaximize = value == WindowState.Normal;
                    this.RaisePropertyChanged(() => WindowState);
                }
			}
		}
		#endregion
        
		#region IsMaximized 変更通知プロパティ

		private bool _IsMaximized;

		public bool IsMaximized
		{
			get { return this._IsMaximized; }
			set
			{
				if (this._IsMaximized != value)
				{
					this._IsMaximized = value;
                    //this.RaisePropertyChanged();
                    this.RaisePropertyChanged(() => IsMaximized);
				}
			}
		}

		#endregion

		#region CanMaximize 変更通知プロパティ

		private bool _CanMaximize = true;

		public bool CanMaximize
		{
			get { return this._CanMaximize; }
			set
			{
				if (this._CanMaximize != value)
				{
					this._CanMaximize = value;
                    //this.RaisePropertyChanged();
                    this.RaisePropertyChanged(() => CanMaximize);
                }
			}
		}

		#endregion

		#region CanMinimize 変更通知プロパティ

		private bool _CanMinimize = true;

		public bool CanMinimize
		{
			get { return this._CanMinimize; }
			set
			{
				if (this._CanMinimize != value)
				{
					this._CanMinimize = value;
                    //this.RaisePropertyChanged();
                    this.RaisePropertyChanged(() => CanMinimize);
                }
			}
		}

		#endregion

		#region CanNormalize 変更通知プロパティ

		private bool _CanNormalize = false;

		public bool CanNormalize
		{
			get { return this._CanNormalize; }
			set
			{
				if (this._CanNormalize != value)
				{
					this._CanNormalize = value;
                    //this.RaisePropertyChanged();
                    this.RaisePropertyChanged(() => CanNormalize);
                }
			}
		}

		#endregion

        #region Loadedメソッド
        /// <summary>
        /// Loadedメソッド
        /// </summary>
        protected override void LoadedHandlerOverride(object sender, RoutedEventArgs e)
        {
            this.config = this.model.OpenConfig();

            DataLogistics.Instance.SetValue(ApplicationEnum.Theme, this.config.Accent);


            this.globalConfig = this.model.LoadGrobalConfig();

            if (this.globalConfig == null)
            {
                this.Config();
            }

            if (this.globalConfig != null && this.globalConfig.EMail != null)
            {
                this.Propertys.GravatarId = this.globalConfig.EMail;
            }
        }
        #endregion

        #region Closedメソッド
        /// <summary>
        /// Closedメソッド
        /// </summary>
        /// <param name="sender">イベント元</param>
        /// <param name="e">パラメーター</param>
        protected override void ClosedHandlerOverride(object sender, EventArgs e)
        {
            this.model.SaveConfig(this.config);
        }
        #endregion

        #region 設定変更コマンド
        /// <summary>
        /// 設定変更コマンド
        /// </summary>
        [Command]
        private void Config()
        {
            /*
            WindowOpenMessage message = this.Messenger.GetResponse<WindowOpenMessage>(new WindowOpenMessage
            {
                MessageKey = "OpenWindow",
                WindowType = WindowTypeEnum.CONFIG,
                Parameter = this.globalConfig,
            });

            if (message.Response != null)
            {
                RepositoryEntity entity = (RepositoryEntity)message.Response.Result;
            }
            */
        }
        #endregion


        [Command]
        private void ChangePurple()
		{
            this.config.Accent = AccentEnum.Purple;
            DataLogistics.Instance.SetValue(ApplicationEnum.Theme, this.config.Accent);
		}
        
        [Command]
        private void ChangeBlue()
		{
            this.config.Accent = AccentEnum.Blue;
            DataLogistics.Instance.SetValue(ApplicationEnum.Theme, this.config.Accent);
        }
        
        [Command]
        private void ChangeOrange()
		{
            this.config.Accent = AccentEnum.Orange;
            DataLogistics.Instance.SetValue(ApplicationEnum.Theme, this.config.Accent);
        }
	}
    #endregion

    #region プロパティクラス
    /// <summary>
    /// プロパティクラス
    /// </summary>
    public class MainWindowViewModelProperty : TacticsProperty
    {
        /// <summary>
        /// Gravatarアイコン表示用ID
        /// </summary>
        public virtual string GravatarId { get; set; }
    }
    #endregion

    #region コマンドクラス
    /// <summary>
    /// コマンドクラス
    /// </summary>
    public class MainWindowViewModelCommand
    {
        /// <summary>
        /// 設定変更
        /// </summary>
        public TacticsCommand Config { get; private set; }

        /// <summary>
        /// リポジトリの登録
        /// </summary>
        public TacticsCommand ChangePurple { get; private set; }
        
        /// <summary>
        /// リポジトリの登録
        /// </summary>
        public TacticsCommand ChangeBlue { get; private set; }

        /// <summary>
        /// リポジトリの登録
        /// </summary>
        public TacticsCommand ChangeOrange { get; private set; }
    }
    #endregion
}
