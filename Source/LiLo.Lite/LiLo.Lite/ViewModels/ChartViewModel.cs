﻿//-----------------------------------------------------------------------
// <copyright file="ChartViewModel.cs" company="InternetWideWorld.com">
// Copyright (c) George Leithead, InternetWideWorld.  All rights reserved.
//   THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
//   OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
//   LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
//   FITNESS FOR A PARTICULAR PURPOSE.
// </copyright>
// <summary>
//   Chart view model.
// </summary>
//-----------------------------------------------------------------------

namespace LiLo.Lite.ViewModels
{
	using System;
	using System.Globalization;
	using System.Linq;
	using System.Threading.Tasks;
	using Lilo.Lite;
	using LiLo.Lite.Models.Markets;
	using LiLo.Lite.ViewModels.Base;
	using Xamarin.Forms;

	/// <summary>Chart view model.</summary>
	[QueryProperty("Symbol", "symbol")]
	public class ChartViewModel : ViewModelBase
	{
		private MarketsModel selectedItem;
		private HtmlWebViewSource tradingViewChart = new HtmlWebViewSource();

		/// <summary>Initialises a new instance of the <see cref="ChartViewModel"/> class.</summary>
		public ChartViewModel()
		{
			this.IsBusy = true;
			this.IsBusy = false;
		}

		public string Symbol
		{
			set
			{
				string symbol = Uri.UnescapeDataString(value);
				this.SelectedItem = this.MarketsHelperService.MarketsList.Where(m => m.SymbolString == symbol).First();
				this.Title = symbol;
			}
		}

		public MarketsModel SelectedItem
		{
			get => this.selectedItem;
			set
			{
				this.selectedItem = value;
				this.NotifyPropertyChanged(() => this.SelectedItem);
				this.TradingViewChart = new HtmlWebViewSource();
			}
		}

		public HtmlWebViewSource TradingViewChart
		{
			get => this.tradingViewChart;
			set
			{
				if (this.SelectedItem == null)
				{
					return;
				}

				string symbol = $"BINANCE:{this.SelectedItem.SymbolString}";
				string colorTheme = Application.Current.UserAppTheme == OSAppTheme.Dark ? "dark" : "light";
				this.tradingViewChart = new HtmlWebViewSource() { Html = GlobalSettings.TradingViewWebViewSource.Html.Replace("XX0XX", TimeZoneInfo.Local.ToString()).Replace("XX1XX", colorTheme).Replace("XX2XX", CultureInfo.CurrentCulture.IetfLanguageTag.Substring(0, 2)).Replace("XX3XX", symbol) };
				this.NotifyPropertyChanged(() => this.TradingViewChart);
			}
		}
	}
}