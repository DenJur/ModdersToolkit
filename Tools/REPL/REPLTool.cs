﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ModdersToolkit.REPL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.UI;

namespace ModdersToolkit.Tools.REPL
{
	class REPLTool : Tool
	{
		internal static REPLBackend replBackend;
		//internal static UserInterface ModdersToolkitUserInterface;
		internal static REPLUI moddersToolkitUI;
		internal static bool EyedropperActive;

		internal override void Initialize()
		{
			replBackend = new REPLBackend();
			toggleTooltip = "Click to toggle C# REPL";
		}

		internal override void ClientInitialize()
		{
			userInterface = new UserInterface();
			moddersToolkitUI = new REPLUI(userInterface);
			moddersToolkitUI.Activate();
			userInterface.SetState(moddersToolkitUI);
		}

		//internal override void ScreenResolutionChanged()
		//{
		//	ModdersToolkitUserInterface.Recalculate();
		//}
		//internal override void UIUpdate()
		//{
		//	if (visible)
		//	{
		//		userInterface.Update(Main._drawInterfaceGameTime);
		//	}
		//}
		internal override void UIDraw()
		{
			if (visible)
			{
				moddersToolkitUI.Draw(Main.spriteBatch);
				if (EyedropperActive)
				{
					Point tileCoords = Main.MouseWorld.ToTileCoordinates();
					Vector2 worldCoords = tileCoords.ToVector2() * 16;
					Vector2 screenCoords = worldCoords - Main.screenPosition;

					DrawBorderedRect(Main.spriteBatch, Color.LightBlue * 0.1f, Color.Blue * 0.3f, screenCoords, new Vector2(16,16), 5);

					if(!Main.LocalPlayer.mouseInterface && Main.mouseLeft)
					{
						EyedropperActive = false;

						Tools.REPL.REPLTool.moddersToolkitUI.codeTextBox.Write($"Main.tile[{tileCoords.X},{tileCoords.Y}]");
						// can't get this to work. Tools.REPL.REPLTool.moddersToolkitUI.codeTextBox.Focus();
					}

					Main.LocalPlayer.mouseInterface = true;
				}
			}
		}
		internal override void Toggled()
		{
			Main.drawingPlayerChat = false;
			if (visible)
			{
				Tools.REPL.REPLTool.moddersToolkitUI.codeTextBox.Focus();
			}
			if (!visible)
			{
				Tools.REPL.REPLTool.moddersToolkitUI.codeTextBox.Unfocus();
			}
		}

		// A helper method that draws a bordered rectangle. 
		public static void DrawBorderedRect(SpriteBatch spriteBatch, Color color, Color borderColor, Vector2 position, Vector2 size, int borderWidth)
		{
			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y), color);
			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X - borderWidth, (int)position.Y - borderWidth, (int)size.X + borderWidth * 2, borderWidth), borderColor);
			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X - borderWidth, (int)position.Y + (int)size.Y, (int)size.X + borderWidth * 2, borderWidth), borderColor);
			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X - borderWidth, (int)position.Y, (int)borderWidth, (int)size.Y), borderColor);
			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X + (int)size.X, (int)position.Y, (int)borderWidth, (int)size.Y), borderColor);
		}
	}
}
