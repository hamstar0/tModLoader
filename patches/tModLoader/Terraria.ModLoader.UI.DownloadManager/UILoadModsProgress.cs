﻿using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.Engine;

namespace Terraria.ModLoader.UI.DownloadManager
{
	internal class UILoadModsProgress : UIProgress
	{
		public int modCount;

		private UIText subProgress;
		private string stageText;

		public override void OnInitialize() {
			base.OnInitialize();
			subProgress = new UIText("", 0.5f, true) {
				Top = { Pixels = 65 },
				HAlign = 0.5f,
				VAlign = 0.5f
			};
			Append(subProgress);
			_cancelButton.Remove();
		}

		public override void OnActivate() {
			base.OnActivate();
			gotoMenu = 0;
			OnCancel += CancelLoadingMods;
			ModLoader.BeginLoad();
			GLCallLocker.ActionsAreSpeedrun = true;
		}

		public override void OnDeactivate() {
			base.OnDeactivate();
			GLCallLocker.ActionsAreSpeedrun = false;
		}

		public override void Update(GameTime gameTime) {
			base.Update(gameTime);
			GLCallLocker.SpeedrunActions();
		}

		public string SubProgressText {
			set => subProgress?.SetText(value);
		}

		public void SetLoadStage(string stageText, int modCount = -1) {
			this.stageText = stageText;
			this.modCount = modCount;
			if (modCount < 0) SetProgressText(Language.GetTextValue(stageText));
			Progress = 0;
			SubProgressText = "";
		}

		private void SetProgressText(string text) {
			Logging.tML.Info(text);
			if (Main.dedServ) Console.WriteLine(text);
			else DisplayText = text;
		}

		public void SetCurrentMod(int i, string mod) {
			SetProgressText(Language.GetTextValue(stageText, mod));
			Progress = i / (float)modCount;
		}

		private void CancelLoadingMods() {
			// TODO Cancelling mods through button to be determined.
			//ModLoader.skipLoad = false;
			//Interface.loadModsProgress.SetLoadStage("Loading Cancelled");
		}
	}
}
