using System.Collections.Generic;

namespace Config
{
    public static class Processor
    {
        public static readonly LoadErrors Errors = new LoadErrors();

        public static void Process(Config.Stream os)
        {
            var configNulls = new List<string>
            {
                "item.item",
            };
            for(;;)
            {
                var csv = os.ReadCfg();
                if (csv == null)
                    break;
                switch(csv)
                {
                    case "item.item":
                        configNulls.Remove(csv);
                        Config.Item.DataItem.Initialize(os, Errors);
                        break;
                    default:
                        Errors.ConfigDataAdd(csv);
                        break;
                }
            }
            foreach (var csv in configNulls)
                Errors.ConfigNull(csv);
        }

    }
}
