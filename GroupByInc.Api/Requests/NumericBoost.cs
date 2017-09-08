using Newtonsoft.Json;

namespace GroupByInc.Api.Requests
{
    public class NumericBoost
    {
        public static readonly double DEFAULT_STRENGTH = 1d;

        [ JsonProperty("name") ] private string _name;
        
        [ JsonProperty("terms") ] private bool _inverted;
        
        [ JsonProperty("strength") ]private double _strength = DEFAULT_STRENGTH;

        public string GetName()
        {
            return _name;
        }

        public NumericBoost SetName(string name)
        {
            _name = name;
            return this;
        }

        public bool IsInverted()
        {
            return _inverted;
        }

        public NumericBoost SetInverted(bool inverted)
        {
            _inverted = inverted;
            return this;
        }

        public double GetStrength()
        {
            return _strength;
        }

        public NumericBoost SetStrength(double strength)
        {
            _strength = strength;
            return this;
        }
    }
}