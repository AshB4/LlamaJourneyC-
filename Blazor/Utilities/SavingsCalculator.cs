//using Plotly.Blazor;
//using Plotly.Blazor.LayoutLib;
//using Plotly.Blazor.Traces;
//using Plotly.Blazor.Traces.ScatterLib;

//namespace Financr.Utils
//{
//    public class PlotlySavingsGrapher
//    {
//        private IList<ITrace> _data;
//        private static string _hoverTemplate = "£ %{y:,.2f} <br> Month %{x}";

//        public SavingsCalculator SavingsCalculator { get; protected set; }

//        public PlotlySavingsGrapher(SavingsCalculator savingsCalculator)
//        {
//            Chart = new PlotlyChart();
//            _data = new List<ITrace>();
//            SavingsCalculator = savingsCalculator;
//            Config = new Config(){ AutoSizable = true, Responsive = true };
//            Layout = new Layout()
//            {
//                BarMode = BarModeEnum.Overlay,
//                Margin = new Margin(){AutoExpand = false, B=20,L=50,R=10,T=10}
//            };
//        }

//        public PlotlyChart Chart;
//        public Config Config { get; set; }
//        public Layout Layout { get; set; } 

//        public IList<ITrace> Data {
//            get => BuildChart();
//            set => _data = value;
//        }

//        public IList<ITrace> BuildChart()
//        {
//            _data.Clear();
//            _data.Add(new Bar
//            {
//                Name = "Pot",
//                Y = BuildPotSeries(),
//                HoverInfo = Plotly.Blazor.Traces.BarLib.HoverInfoFlag.Y,
//                HoverTemplate = _hoverTemplate,
//            });
//            _data.Add(new Bar()
//            {
//                Name = "Deposited",
//                Y = BuildDepositsSeries(),
//                HoverInfo = Plotly.Blazor.Traces.BarLib.HoverInfoFlag.Y,
//                HoverTemplate = _hoverTemplate,
//            });
//            return _data;
//        }
//        private IList<object> BuildDepositsSeries()
//        {
//            var bar = this.SavingsCalculator.Deposits.Select(x => x.BankerRound()).Cast<object>().ToList();
//            return bar;
//        }

//        private IList<object> BuildPotSeries()
//        {
//            var bar = this.SavingsCalculator.Pot.Select(x => x.BankerRound()).Cast<object>().ToList();
//            return bar;
//        }
//    }
//}namespace Financr.Utils


    public class SavingsCalculator
{
    public decimal StartingBalance { get; set; }
    public decimal ExpectedAnnualReturn { get; set; }
    public decimal ExpectedMonthlyDeposit { get; set; }
    public decimal ExpectedMonthlyDepositIncrease { get; set; }
    public int Months { get; set; }
    public int Years { get; set; }
    public int TotalMonths => this.Years * 12 + Months;

    public IEnumerable<Decimal> Pot =>
        MathHelpers.CompoundInterest(
            this.StartingBalance,
            this.ExpectedAnnualReturn,
            this.ExpectedMonthlyDeposit,
            this.ExpectedMonthlyDepositIncrease).Take(this.TotalMonths);
    public IEnumerable<Decimal> Deposits =>
        MathHelpers.CompoundInterest(
            this.StartingBalance,
            0,
            this.ExpectedMonthlyDeposit,
            this.ExpectedMonthlyDepositIncrease).Take(this.TotalMonths);
}

public static class MathHelpers
{
    public static decimal BankerRound(this decimal foo)
    {
        return decimal.Round(foo, 2, MidpointRounding.AwayFromZero);
    }

    public static IEnumerable<decimal> CompoundInterest(decimal startingBalance, decimal expectedAnnualReturn, decimal expectedMonthlyDeposit, decimal expectedAnnualDepositIncrease)
    {
        var monthlyInterest = 1 + expectedAnnualReturn / (1200);
        var currentAmount = startingBalance;

        var annualInterest = 1 + expectedAnnualDepositIncrease / (100);
        var adjustedMonthlyAmount = expectedMonthlyDeposit;
        var months = 0;
        while (true)
        {
            months++;
            if (months % 12 == 0)
            {
                adjustedMonthlyAmount *= annualInterest;
            }
            currentAmount = currentAmount * monthlyInterest + adjustedMonthlyAmount;
            yield return currentAmount;
        }
    }
}
