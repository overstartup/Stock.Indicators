﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Skender.Stock.Indicators;

namespace Internal.Tests
{
    [TestClass]
    public class ZigZag : TestBase
    {

        [TestMethod]
        public void StandardClose()
        {

            List<ZigZagResult> results =
                quotes.GetZigZag(EndType.Close, 3)
                .ToList();

            // assertions

            // proper quantities
            // should always be the same number of results as there is quotes
            Assert.AreEqual(502, results.Count);
            Assert.AreEqual(234, results.Where(x => x.ZigZag != null).Count());
            Assert.AreEqual(234, results.Where(x => x.RetraceHigh != null).Count());
            Assert.AreEqual(221, results.Where(x => x.RetraceLow != null).Count());
            Assert.AreEqual(14, results.Where(x => x.PointType != null).Count());

            // sample values
            ZigZagResult r0 = results[249];
            Assert.AreEqual(null, r0.ZigZag);
            Assert.AreEqual(null, r0.RetraceHigh);
            Assert.AreEqual(null, r0.RetraceLow);
            Assert.AreEqual(null, r0.PointType);

            ZigZagResult r1 = results[277];
            Assert.AreEqual(248.13m, r1.ZigZag);
            Assert.AreEqual(272.248m, r1.RetraceHigh);
            Assert.AreEqual(248.13m, r1.RetraceLow);
            Assert.AreEqual("L", r1.PointType);

            ZigZagResult r2 = results[483];
            Assert.AreEqual(272.52m, r2.ZigZag);
            Assert.AreEqual(272.52m, r2.RetraceHigh);
            Assert.AreEqual(248.799m, r2.RetraceLow);
            Assert.AreEqual("H", r2.PointType);

            ZigZagResult r3 = results[439];
            Assert.AreEqual(276.0133m, Math.Round((decimal)r3.ZigZag, 4));
            Assert.AreEqual(280.9158m, Math.Round((decimal)r3.RetraceHigh, 4));
            Assert.AreEqual(264.5769m, Math.Round((decimal)r3.RetraceLow, 4));
            Assert.AreEqual(null, r3.PointType);

            ZigZagResult r4 = results[500];
            Assert.AreEqual(241.4575m, Math.Round((decimal)r4.ZigZag, 4));
            Assert.AreEqual(246.7933m, Math.Round((decimal)r4.RetraceHigh, 4));
            Assert.AreEqual(null, r4.RetraceLow);
            Assert.AreEqual(null, r4.PointType);

            ZigZagResult r5 = results[501];
            Assert.AreEqual(245.28m, r5.ZigZag);
            Assert.AreEqual(245.28m, r5.RetraceHigh);
            Assert.AreEqual(null, r5.RetraceLow);
            Assert.AreEqual(null, r5.PointType);
        }

        [TestMethod]
        public void StandardHighLow()
        {

            List<ZigZagResult> results =
                quotes.GetZigZag(EndType.HighLow, 3)
                .ToList();

            // assertions

            // proper quantities
            // should always be the same number of results as there is quotes
            Assert.AreEqual(502, results.Count);
            Assert.AreEqual(463, results.Where(x => x.ZigZag != null).Count());
            Assert.AreEqual(462, results.Where(x => x.RetraceHigh != null).Count());
            Assert.AreEqual(445, results.Where(x => x.RetraceLow != null).Count());
            Assert.AreEqual(31, results.Where(x => x.PointType != null).Count());

            // sample values
            ZigZagResult r0 = results[38];
            Assert.AreEqual(null, r0.ZigZag);
            Assert.AreEqual(null, r0.RetraceHigh);
            Assert.AreEqual(null, r0.RetraceLow);
            Assert.AreEqual(null, r0.PointType);

            ZigZagResult r1 = results[277];
            Assert.AreEqual(252.9550m, r1.ZigZag);
            Assert.AreEqual(262.8054m, Math.Round((decimal)r1.RetraceHigh, 4));
            Assert.AreEqual(245.4467m, Math.Round((decimal)r1.RetraceLow, 4));
            Assert.AreEqual(null, r1.PointType);

            ZigZagResult r2 = results[316];
            Assert.AreEqual(249.48m, r2.ZigZag);
            Assert.AreEqual(258.34m, r2.RetraceHigh);
            Assert.AreEqual(249.48m, r2.RetraceLow);
            Assert.AreEqual("L", r2.PointType);

            ZigZagResult r3 = results[456];
            Assert.AreEqual(261.3325m, Math.Round((decimal)r3.ZigZag, 4));
            Assert.AreEqual(274.3419m, Math.Round((decimal)r3.RetraceHigh, 4));
            Assert.AreEqual(256.1050m, Math.Round((decimal)r3.RetraceLow, 4));
            Assert.AreEqual(null, r3.PointType);

            ZigZagResult r4 = results[500];
            Assert.AreEqual(246.73m, Math.Round((decimal)r4.ZigZag, 4));
            Assert.AreEqual(246.73m, r4.RetraceHigh);
            Assert.AreEqual(238.3867m, Math.Round((decimal)r4.RetraceLow, 4));
            Assert.AreEqual("H", r4.PointType);

            ZigZagResult r5 = results[501];
            Assert.AreEqual(242.87m, r5.ZigZag);
            Assert.AreEqual(null, r5.RetraceHigh);
            Assert.AreEqual(242.87m, r5.RetraceLow);
            Assert.AreEqual(null, r5.PointType);
        }

        [TestMethod]
        public void BadData()
        {
            IEnumerable<ZigZagResult> r1 = Indicator.GetZigZag(badQuotes, EndType.Close);
            Assert.AreEqual(502, r1.Count());

            IEnumerable<ZigZagResult> r2 = Indicator.GetZigZag(badQuotes, EndType.HighLow);
            Assert.AreEqual(502, r2.Count());
        }

        [TestMethod]
        public void SchrodingerScenario()
        {
            IEnumerable<Quote> h = TestData.GetZigZag();

            IEnumerable<ZigZagResult> r1 = Indicator.GetZigZag(h, EndType.Close, 0.25m);
            Assert.AreEqual(342, r1.Count());

            // first period has High/Low that exceeds threhold
            // where it is both a H and L pivot simultaenously
            IEnumerable<ZigZagResult> r2 = Indicator.GetZigZag(h, EndType.HighLow, 3);
            Assert.AreEqual(342, r2.Count());
        }

        [TestMethod]
        public void Exceptions()
        {
            // bad lookback period
            Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
                Indicator.GetZigZag(quotes, EndType.Close, 0));

            // insufficient quotes
            Assert.ThrowsException<BadQuotesException>(() =>
                Indicator.GetZigZag(TestData.GetDefault(1)));
        }

        // See Issue #510
        [TestMethod]
        public void Issue510()
        {
            string json = File.ReadAllText("data/issue510-B.json");
            dynamic data = JsonConvert.DeserializeObject(json);

            List<Quote> quotes = new();

            foreach (dynamic d in data)
            {
                dynamic q = d.QuotesIndicators;
                Quote quote = new()
                {
                    Date = q.Date,
                    Open = q.Open,
                    High = q.High,
                    Low = q.Low,
                    Close = q.Close
                };

                quotes.Add(quote);
            }

            int size = quotes.Count;
            List<ZigZagResult> results1 = quotes.GetZigZag(EndType.HighLow, 1).ToList();
            List<ZigZagResult> results10 = quotes.GetZigZag(EndType.HighLow, 10).ToList();
            List<Quote> quotesList = quotes.OrderBy(x => x.Date).ToList();

            for (int i = 0; i < size; i++)
            {
                Quote q = quotesList[i];
                ZigZagResult r10 = results10[i];
                ZigZagResult r1 = results1[i];

                Console.WriteLine("{0},{1:N5},{2:N5},{3:N5},{4:N5},{5:N5},{6:N5}",
                    q.Date, q.Open, q.High, q.Low, q.Close, r1.ZigZag, r10.ZigZag);
            }

            Assert.Fail();
        }

    }
}
