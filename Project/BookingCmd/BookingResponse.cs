namespace BookingCmd {
    public class Response {
        public int BizCode { get; set; }
        public string Message { get; set; }
        public Data Data { get; set; }
    }

    public class Category {
        public string Id { get; set; }
        public string Name { get; set; }
        public Tournament Tournament { get; set; }
    }

    public class Data {
        public string ShareCode { get; set; }
        public string ShareURL { get; set; }
        public List<Outcome> Outcomes { get; set; }
    }

    public class Market {
        public string Id { get; set; }
        public int Product { get; set; }
        public string Desc { get; set; }
        public int Status { get; set; }
        public string Group { get; set; }
        public string MarketGuide { get; set; }
        public int Favourite { get; set; }
        public List<Outcome> Outcomes { get; set; }
    }

    public class Outcome {
        public string EventId { get; set; }
        public string GameId { get; set; }
        public string ProductStatus { get; set; }
        public object EstimateStartTime { get; set; }
        public int Status { get; set; }
        public string SetScore { get; set; }
        public List<string> GameScore { get; set; }
        public string Period { get; set; }
        public string MatchStatus { get; set; }
        public string PlayedSeconds { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public Sport Sport { get; set; }
        public List<Market> Markets { get; set; }
        public string BookingStatus { get; set; }
        public string Id { get; set; }
        public string Odds { get; set; }
        public string Probability { get; set; }
        public int IsActive { get; set; }
        public string Desc { get; set; }
    }

    public class Sport {
        public string Id { get; set; }
        public string Name { get; set; }
        public Category Category { get; set; }
    }

    public class Tournament {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
