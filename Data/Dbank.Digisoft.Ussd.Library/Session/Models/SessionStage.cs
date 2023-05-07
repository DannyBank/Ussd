using System.ComponentModel;

namespace Dbank.Digisoft.Ussd.SDK.Session.Models
{
    [DefaultValue(NotStarted)]
    public enum SessionStage
    {
        Start = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4,
        Level5 = 5,
        Level6 = 6,
        Level7 = 7,
        Level8 = 8,
        Level9 = 9,
        Level10 = 10,
        Level11 = 11,
        Level12 = 12,
        Final = 100,
        NotStarted =999
    }
}