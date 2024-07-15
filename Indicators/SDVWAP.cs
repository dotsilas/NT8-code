#region Using declarations
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using NinjaTrader.Cbi;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Chart;
using NinjaTrader.Gui.SuperDom;
using NinjaTrader.Gui.Tools;
using NinjaTrader.Data;
using NinjaTrader.NinjaScript;
using NinjaTrader.Core.FloatingPoint;
using NinjaTrader.NinjaScript.DrawingTools;
#endregion

//This namespace holds Indicators in this folder and is required. Do not change it. 
namespace NinjaTrader.NinjaScript.Indicators.SDFree
{
	public class SDVWAP : Indicator
	{
		#region values
			
			double typicalPrice 							= 0;
			double volumePerTypicalPrice 					= 0;
			double cumulativeVolumePerTypicalPrice 			= 0;
			double cumulativeVolume 						= 0;
			double vwap 									= 0;
			
		#endregion
		
		protected override void OnStateChange()
		{
			
			if (State == State.SetDefaults)
			{
				Description									= @"vwap indicator";
				Name										= "SDVWAP";
				Calculate									= Calculate.OnBarClose;
				IsOverlay									= false;
				DisplayInDataBox							= true;
				DrawOnPricePanel							= true;
				DrawHorizontalGridLines						= true;
				DrawVerticalGridLines						= true;
				PaintPriceMarkers							= true;
				ScaleJustification							= NinjaTrader.Gui.Chart.ScaleJustification.Right;
				//Disable this property if your indicator requires custom values that cumulate with each new market data event. 
				//See Help Guide for additional information.
				IsSuspendedWhileInactive					= true;
				
				#region 00. Sessions Times
				
					customCalculation							= false;
					startCalculation 							= DateTime.Parse("08:30");
				
				#endregion
				
				#region Plots
				
					AddPlot(new Stroke(Brushes.Yellow, 1), PlotStyle.Line, "PlotVWAP");
				
				#endregion
			}
			else if (State == State.Configure)
			{
			}
		}

		protected override void OnBarUpdate()
		{
			//Add your custom indicator logic here.
			typicalPrice = (High[0] + Low[0] + Close[0]) / 3;
			
			if (customCalculation)
			{
				// start from custom
			}
			else
			{
				// start from session
				if (Bars.IsFirstBarOfSession)
				{
					cumulativeVolumePerTypicalPrice = VOL()[0] * typicalPrice;
					cumulativeVolume = VOL()[0];
				}
				else
				{
					cumulativeVolumePerTypicalPrice = cumulativeVolumePerTypicalPrice + (VOL()[0] * typicalPrice);
					cumulativeVolume = cumulativeVolume + VOL()[0];
				}
				
				vwap = cumulativeVolumePerTypicalPrice / cumulativeVolume;
				PlotVWAP[0] = vwap;
			}
		}

		#region Properties
		[NinjaScriptProperty]
		[Display(Name="Custom calculation", Order=0, GroupName="00. Sessions Times")]
		public bool customCalculation
		{ get; set; }
		
		[NinjaScriptProperty]
		[PropertyEditor("NinjaTrader.Gui.Tools.TimeEditorKey")]
		[Display(Name="Starting time", Description="Time to start to calculate VWAP", Order=1, GroupName="00. Sessions Times")]
		public DateTime startCalculation
		{ get; set; }
		
		// ----------------------------
		// Plots
		// ----------------------------
		[Browsable(false)]
		[XmlIgnore]
		public Series<double> PlotVWAP
		{
			get { return Values[0]; }
		}
		#endregion

	}
}

#region NinjaScript generated code. Neither change nor remove.

namespace NinjaTrader.NinjaScript.Indicators
{
	public partial class Indicator : NinjaTrader.Gui.NinjaScript.IndicatorRenderBase
	{
		private SDFree.SDVWAP[] cacheSDVWAP;
		public SDFree.SDVWAP SDVWAP(bool customCalculation, DateTime startCalculation)
		{
			return SDVWAP(Input, customCalculation, startCalculation);
		}

		public SDFree.SDVWAP SDVWAP(ISeries<double> input, bool customCalculation, DateTime startCalculation)
		{
			if (cacheSDVWAP != null)
				for (int idx = 0; idx < cacheSDVWAP.Length; idx++)
					if (cacheSDVWAP[idx] != null && cacheSDVWAP[idx].customCalculation == customCalculation && cacheSDVWAP[idx].startCalculation == startCalculation && cacheSDVWAP[idx].EqualsInput(input))
						return cacheSDVWAP[idx];
			return CacheIndicator<SDFree.SDVWAP>(new SDFree.SDVWAP(){ customCalculation = customCalculation, startCalculation = startCalculation }, input, ref cacheSDVWAP);
		}
	}
}

namespace NinjaTrader.NinjaScript.MarketAnalyzerColumns
{
	public partial class MarketAnalyzerColumn : MarketAnalyzerColumnBase
	{
		public Indicators.SDFree.SDVWAP SDVWAP(bool customCalculation, DateTime startCalculation)
		{
			return indicator.SDVWAP(Input, customCalculation, startCalculation);
		}

		public Indicators.SDFree.SDVWAP SDVWAP(ISeries<double> input , bool customCalculation, DateTime startCalculation)
		{
			return indicator.SDVWAP(input, customCalculation, startCalculation);
		}
	}
}

namespace NinjaTrader.NinjaScript.Strategies
{
	public partial class Strategy : NinjaTrader.Gui.NinjaScript.StrategyRenderBase
	{
		public Indicators.SDFree.SDVWAP SDVWAP(bool customCalculation, DateTime startCalculation)
		{
			return indicator.SDVWAP(Input, customCalculation, startCalculation);
		}

		public Indicators.SDFree.SDVWAP SDVWAP(ISeries<double> input , bool customCalculation, DateTime startCalculation)
		{
			return indicator.SDVWAP(input, customCalculation, startCalculation);
		}
	}
}

#endregion
