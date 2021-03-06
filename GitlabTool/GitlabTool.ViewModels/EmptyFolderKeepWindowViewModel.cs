﻿#region License
// <copyright>
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
#endregion

namespace GitlabTool.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Common.Library.Entitys;
    using Gordias.Library.Headquarters;
    using Gordias.Library.Interface;
    using Livet.Messaging.IO;
    using Livet.Messaging.Windows;
    using log4net;

    #region メインクラス
    /// <summary>
    /// 空フォルダの保持ウインドウビューモデル
    /// </summary>
    public class EmptyFolderKeepWindowViewModel : TacticsViewModel<EmptyFolderKeepWindowViewModelProperty, EmptyFolderKeepWindowViewModelCommand>, IWindowParameter
    {
        /// <summary>
        /// ログ
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1311:StaticReadonlyFieldsMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Initializeメソッド
        /// <summary>
        /// Initializeメソッド
        /// </summary>
        public void Initialize()
        {
            RepositoryEntity entity = (RepositoryEntity)this.Parameter;

            if (entity != null)
            {
                this.Propertys.FolderPath = entity.Location;
            }
        }
        #endregion

        #region OKボタン処理
        /// <summary>
        /// OKボタン処理
        /// </summary>
        [Command]
        private void OkButton()
        {
            logger.Info("操作：OKボタン");

            if (string.IsNullOrEmpty(this.Propertys.FolderPath))
            {
                return;
            }
            this.SearchDirectory(this.Propertys.FolderPath);

            this.Messenger.Raise(new WindowActionMessage("WindowControl", WindowAction.Close));
        }
        #endregion

        /// <summary>
        /// 空フォルダに、.gitkeepファイルを作成する。
        /// 空でないフォルダに、.gitkeepファイルが有ったら削除する。
        /// </summary>
        /// <param name="path">パス</param>
        private void SearchDirectory(string path)
        {
            Action<string> checkDeleteGitKeep = (string check) =>
                {
                    if (System.IO.File.Exists(check + @"\.gitkeep"))
                    {
                        System.IO.File.Delete(check + @"\.gitkeep");
                    }
                };

            string[] directorys = System.IO.Directory.GetDirectories(path, "*", System.IO.SearchOption.TopDirectoryOnly);
            string[] files = System.IO.Directory.GetFiles(path, "*", System.IO.SearchOption.TopDirectoryOnly);

            int filecnt = 0;

            foreach (string file in files)
            {
                if (file.LastIndexOf(".gitkeep") != (file.Length - ".gitkeep".Length))
                {
                    filecnt++;
                }
            }

            if (directorys.Length == 0 && filecnt == 0)
            {
                if (!System.IO.File.Exists(path + @"\.gitkeep"))
                {
                    System.IO.FileStream fs = System.IO.File.Open(path + @"\.gitkeep", System.IO.FileMode.Create);

                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            else if (directorys.Length > 0)
            {
                foreach (string directory in directorys)
                {
                    if (directory.LastIndexOf(".git") == -1)
                    {
                        this.SearchDirectory(directory);
                    }
                }
                checkDeleteGitKeep(path);
            }
            else
            {
                checkDeleteGitKeep(path);
            }
        }

        #region Cancelボタン処理
        /// <summary>
        /// Cancelボタン処理
        /// </summary>
        [Command]
        private void CancelButton()
        {
            logger.Info("操作：Cancelボタン");

            this.Messenger.Raise(new WindowActionMessage("WindowControl", WindowAction.Close));
        }
        #endregion

        /// <summary>
        /// フォルダーの選択
        /// </summary>
        /// <param name="message">フォルダー選択メッセージ</param>
        public void FolderSelected(FolderSelectionMessage message)
        {
            this.Propertys.FolderPath = message.Response;
        }

        /// <summary>
        /// パラメーター
        /// </summary>
        public object Parameter { get; set; }
    }
    #endregion

    #region プロパティクラス
    /// <summary>
    /// プロパティクラス
    /// </summary>
    public class EmptyFolderKeepWindowViewModelProperty : TacticsProperty
    {
        /// <summary>
        /// FolderPath
        /// </summary>
        public virtual string FolderPath { get; set; }
    }
    #endregion

    #region コマンドクラス
    /// <summary>
    /// コマンドクラス
    /// </summary>
    public class EmptyFolderKeepWindowViewModelCommand
    {
        /// <summary>
        /// OkButtonコマンド
        /// </summary>
        public TacticsCommand OkButton { get; private set; }

        /// <summary>
        /// CancelButtonコマンド
        /// </summary>
        public TacticsCommand CancelButton { get; private set; }
    }
    #endregion
}
