using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;

namespace VirtualKeyboard.Models{
	public abstract class VLayout{
        public static readonly FontFamily MIconFont = new FontFamily(new Uri("pack://application:,,,/fonts/#FontAwesome"), "./#FontAwesome");

		/// <summary> Загрузка слоя XML </summary>
		/// <param name="fName"></param>
		/// <param name="uiGrid"></param>
		/// <param name="uiWindow"></param>
		/// <returns></returns>
        public static bool Load(string fName,Grid uiGrid,Window uiWindow){
            try{ 
			    //..........................................
			    //Загрузка слоя XML
			    var xml = new XmlDocument();
			    xml.Load(RootPath("Layouts\\" + fName + ".xml"));

			    var root = xml.DocumentElement;
			    if(root.ChildNodes.Count == 0) return false;

			    #region Установка размеров окна

			    var sHeight = SystemParameters.WorkArea.Height;
			    var sWidth = SystemParameters.WorkArea.Width;
			
			    uiWindow.Width = double.Parse(root.GetAttribute("width"));
			    uiWindow.Height = double.Parse(root.GetAttribute("height"));

			    #endregion

			    #region Установка расположения окна

			    switch(root.GetAttribute("vpos")){
				    case "top": uiWindow.Top = 20; break;
				    case "center": uiWindow.Top = (sHeight - uiWindow.Height) / 2; break;
				    case "bottom": uiWindow.Top = sHeight - uiWindow.Height - 20; break;
			    }

			    switch(root.GetAttribute("hpos")){
				    case "left": uiWindow.Left = 20; break;
				    case "center": uiWindow.Left = (sWidth - uiWindow.Width) / 2; break;
				    case "right": uiWindow.Left = sWidth - uiWindow.Width - 20; break;
			    }

			    #endregion

			    //..........................................

			    #region Устанавливаем Margin

			    var sMargin = root.GetAttribute("margin");
			    if(!string.IsNullOrEmpty(sMargin)){
				    var aryMargin = sMargin.Split(',');
				    if(aryMargin.Length == 4){
					    uiGrid.Margin = new Thickness(
						    int.Parse(aryMargin[0]), 
						    int.Parse(aryMargin[1]), 
						    int.Parse(aryMargin[2]), 
						    int.Parse(aryMargin[3])
					    );
				    }
			    }

			    #endregion

                //..........................................
                //Reset UI Grid
                uiGrid.Children.Clear();
			    uiGrid.RowDefinitions.Clear();
			    uiGrid.ColumnDefinitions.Clear();

			    //Create all the rows on the main UI Grid
			    for(int i=0; i < root.ChildNodes.Count; i++) uiGrid.RowDefinitions.Add(new RowDefinition(){ Height=new GridLength(1,GridUnitType.Star) });

			    //..........................................
			    //Reset UI Grid
			    int iRow=0;

			    foreach(XmlNode row in root.ChildNodes){
				    //Create Key Row Container
				    var rGrid = CreateGrid();
				    Grid.SetRow(rGrid,iRow);
				    Grid.SetColumn(rGrid,0);
				    uiGrid.Children.Add(rGrid);

				    //Create Keys
				    var iKey=0;
				    foreach(XmlElement key in row.ChildNodes){
					    var sgLen = key.GetAttribute("weight");
					    var gLen = string.IsNullOrEmpty(sgLen) ? 1 : double.Parse(sgLen);

					    rGrid.ColumnDefinitions.Add(new ColumnDefinition{ Width=new GridLength(gLen,GridUnitType.Star) });
					    rGrid.Children.Add(CreateButton(key,iKey));
					    iKey++;
				    }
				    iRow++;
			    }

                return true;
            }catch(Exception e) {
                VLogger.Exception("vLayout.Load", e, fName);
            }

            return false;
        }

		private static Grid CreateGrid(){
			var grid = new Grid();
			grid.RowDefinitions.Add(new RowDefinition { Height=new GridLength(1,GridUnitType.Star) });
			return grid;
		}
		/// <summary>
		/// Создание кнопки на форме
		/// </summary>
		/// <param name="elm"></param>
		/// <param name="col"></param>
		/// <returns></returns>
		private static VButton CreateButton(XmlElement elm,int col){
			string code;
			string title = elm.GetAttribute("text"),fsize = elm.GetAttribute("fsize");

			var btn = new VButton();
            Grid.SetRow(btn,0);
			Grid.SetColumn(btn,col);
            btn.FontFamily = MIconFont;
            btn.Title = title;
            if(!string.IsNullOrEmpty(fsize)) btn.FontSize = double.Parse(fsize);

            switch(elm.Name){
                //.........................................
                case "key":
                    code = elm.GetAttribute("code");
                    var shCode = elm.GetAttribute("shcode");
                    var shText = elm.GetAttribute("shtext");

                    if(!string.IsNullOrEmpty(code)) btn.KbCommand.KbKeys = code.Split(' ');
			        if(!string.IsNullOrEmpty(shCode)) btn.KbCommand.KbShKeys = shCode.Split(' ');
			        if(!string.IsNullOrEmpty(shText)) btn.ShiftText = shText;

                    btn.KbCommand.SendString = elm.GetAttribute("string");
                    btn.KbCommand.ShSendString = elm.GetAttribute("shstring");

                    btn.PreviewMouseLeftButtonDown += BtnTouch_Down;
                    btn.PreviewMouseLeftButtonUp += BtnTouch_Up;
                    btn.PreviewTouchDown += BtnTouch_Down;
                    btn.PreviewTouchUp += BtnTouch_Up;

                    break;
                //.........................................
                case "menu":
                    var menu = new ContextMenu();

                    foreach(XmlElement itm in elm.ChildNodes){
                        var kbCmd = new KeyboardCommand();
                        title = itm.GetAttribute("text");
                        code = itm.GetAttribute("code");

                        if(!string.IsNullOrEmpty(code)) kbCmd.KbKeys = code.Split(' ');
                        kbCmd.SendString = itm.GetAttribute("string");

                        var mItem = new MenuItem { Header = title, Tag = kbCmd };
                        mItem.Click += OnMenuClick;
                        menu.Items.Add(mItem);
                    }

                    btn.ContextMenu = menu;
                    btn.Click += OnMenuButtonPress;
                    break;
            }
        
			return btn;
		}

		private static void BtnTouch_Down(object sender, EventArgs e)
		{
			if(sender is VButton btn)
				KeyLoopHandler.BeginKeypress(btn.KbCommand);
		}
        private static void BtnTouch_Up(object sender, EventArgs e) { KeyLoopHandler.EndKeypress(); }

        private static void OnMenuClick(object sender, RoutedEventArgs e)
        {
	        if(sender is MenuItem mi)
				VKeyboard.ProcessCommand((KeyboardCommand)mi.Tag);
        }

        private static void OnMenuButtonPress(object sender, RoutedEventArgs e)
        {
	        if (!(sender is VButton btn)) return;
	        if(btn.ContextMenu == null) return;
			btn.ContextMenu.IsOpen = true;
        }

        private static string RootPath(string relativePath){
			var rtn = System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName);
			if (rtn == null) return "";
			if(!rtn.EndsWith("\\")) rtn += "\\";
			return rtn + relativePath;
		}
	
		public static string[] GetLayoutList(){
            var rtn = System.IO.Directory.GetFiles(RootPath("Layouts"), "*.xml");

            for(var i=0; i < rtn.Length; i++) rtn[i] = System.IO.Path.GetFileNameWithoutExtension(rtn[i]);

            return rtn; 
		}
	}
}
