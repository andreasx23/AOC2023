using AOC2023.Day07;

namespace AOC2023.Test
{
    public class Day07Part1Test
    {
        //[SetUp]
        //public void Setup()
        //{
        //}

        //[Test]
        //public void FiveOfAKind()
        //{
        //    var test = new Day07Part1.CamelCard("AAAAA", 1);
        //    Assert.IsTrue(test.IsFiveOfAKind());
        //}

        //[Test]
        //public void FourOfAKind()
        //{
        //    var test = new Day07Part1.CamelCard("AA8AA", 1);
        //    Assert.IsTrue(test.IsFourOfAKind());
        //}

        //[Test]
        //public void FullHouse()
        //{
        //    var test = new Day07Part1.CamelCard("23332", 1);
        //    Assert.IsTrue(test.IsFullHouse());
        //}

        //[Test]
        //public void ThreeOfAKind()
        //{
        //    var test = new Day07Part1.CamelCard("TTT98", 1);
        //    Assert.IsTrue(test.IsThreeOfAKind());
        //}

        //[Test]
        //public void TwoPair()
        //{
        //    var test = new Day07Part1.CamelCard("23432", 1);
        //    Assert.IsTrue(test.IsTwoPair());
        //}

        //[Test]
        //public void OnePair()
        //{
        //    var test = new Day07Part1.CamelCard("A23A4", 1);
        //    Assert.IsTrue(test.IsOnePair());
        //}

        //[Test]
        //public void HighCard()
        //{
        //    var test = new Day07Part1.CamelCard("23456", 1);
        //    Assert.IsTrue(test.IsHighCard());
        //}

        //[Test]
        //public void FiveOfAKindBeatFourOfAKind()
        //{
        //    var five = new Day07Part1.CamelCard("AAAAA", 1);
        //    var four = new Day07Part1.CamelCard("AA8AA", 1);
        //    Assert.IsTrue(five.GetHandScore() > four.GetHandScore());
        //}

        //[Test]
        //public void CamelCardComparer()
        //{
        //    var five = new Day07Part1.CamelCard("AAAAA", 1);
        //    var four = new Day07Part1.CamelCard("AA8AA", 1);
        //    var camelCardComparer = new Day07Part1.CamelCardComparer();
        //    Assert.IsTrue(camelCardComparer.Compare(five, four) > 0);
        //}

        //[Test]
        //public void CamelCardComparer2()
        //{
        //    var one = new Day07Part1.CamelCard("KK677", 1);
        //    var two = new Day07Part1.CamelCard("KTJJT", 1);
        //    var camelCardComparer = new Day07Part1.CamelCardComparer();
        //    Assert.IsTrue(camelCardComparer.Compare(one, two) > 0);
        //}

        //[Test]
        //public void CamelCardComparer3()
        //{
        //    var one = new Day07Part1.CamelCard("32T3K", 765);
        //    var two = new Day07Part1.CamelCard("T55J5", 684);
        //    var three = new Day07Part1.CamelCard("KK677", 28);
        //    var four = new Day07Part1.CamelCard("KTJJT", 220);
        //    var five = new Day07Part1.CamelCard("QQQJA", 483);
        //    List<Day07Part1.CamelCard> camelCards = new List<Day07Part1.CamelCard>()
        //    {
        //        one,
        //        two,
        //        three,
        //        four,
        //        five,
        //    };

        //    var camelCardComparer = new Day07Part1.CamelCardComparer();
        //    var sorted = camelCards.OrderBy(x => x, camelCardComparer).ToList();

        //    Assert.IsTrue(sorted[0].Id == "32T3K");
        //    Assert.IsTrue(sorted[1].Id == "KTJJT");
        //    Assert.IsTrue(sorted[2].Id == "KK677");
        //    Assert.IsTrue(sorted[3].Id == "T55J5");
        //    Assert.IsTrue(sorted[4].Id == "QQQJA");

        //    long sum = 0;
        //    for (int i = 0; i < sorted.Count; i++)
        //    {
        //        sum += sorted[i].GetBidScore(i + 1);
        //    }
        //    Assert.IsTrue(sum == 6440);
        //}

        //[Test]
        //public void CamelCardComparer4()
        //{
        //    var one = new Day07Part1.CamelCard("KK677", 1);
        //    var two = new Day07Part1.CamelCard("KTJJT", 1);
        //    var three = new Day07Part1.CamelCard("32T3K", 765);

        //    List<Day07Part1.CamelCard> camelCards = new List<Day07Part1.CamelCard>()
        //    {
        //        one,
        //        two,
        //        three,
        //    };

        //    var camelCardComparer = new Day07Part1.CamelCardComparer();

        //    var sorted = camelCards.OrderBy(x => x, camelCardComparer).ToList();

        //    Assert.IsTrue(sorted[0].Id == "32T3K");
        //    Assert.IsTrue(sorted[1].Id == "KTJJT");
        //    Assert.IsTrue(sorted[2].Id == "KK677");
        //}


        //[Test]
        //public void CamelCardComparer5()
        //{
        //    var one = new Day07Part1.CamelCard("32T3K", 765);
        //    var two = new Day07Part1.CamelCard("T55J5", 684);
        //    var three = new Day07Part1.CamelCard("KK677", 28);
        //    var four = new Day07Part1.CamelCard("KTJJT", 220);

        //    List<Day07Part1.CamelCard> camelCards = new List<Day07Part1.CamelCard>()
        //    {
        //        one,
        //        two,
        //        three,
        //        four,
        //    };

        //    var camelCardComparer = new Day07Part1.CamelCardComparer();

        //    var sorted = camelCards.OrderBy(x => x, camelCardComparer).ThenBy(x => x, camelCardComparer).ToList();

        //    foreach (var item in sorted)
        //    {
        //        var score = item.GetHandScore();
        //    }

        //    Assert.IsTrue(sorted[0].Id == "32T3K");
        //    Assert.IsTrue(sorted[1].Id == "KTJJT");
        //    Assert.IsTrue(sorted[2].Id == "KK677");
        //    Assert.IsTrue(sorted[3].Id == "T55J5");
        //}
    }
}