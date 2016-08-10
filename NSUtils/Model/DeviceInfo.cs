namespace NSUtils.Model
{
    public class DeviceInfo
    {
        public string SDK { get; set; }

        public string Device { get; set; }

        public string Model { get; set; }

        public string Product { get; set; }

        public string ApplicationVersion { get; set; }

        public float BatteryLevel { get; set; }

        public string Network { get; set; }

        public override string ToString()
        {
            return string.Format("SDK: {0} \n Device: {1} \n Model: {2} \n Product: {3} \n ApplicationVersion: {4} \n BatteryLevel: {5} \n Network: {6}",
                SDK, Device, Model, Product, ApplicationVersion, BatteryLevel, Network);
        }
    }
}
