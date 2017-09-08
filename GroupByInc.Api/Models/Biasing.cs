using System.Collections.Generic;
using Newtonsoft.Json;

namespace GroupByInc.Api.Models
{
    /// <summary>
    ///     Query-time biasing
    /// </summary>
    public class Biasing
    {
        [JsonProperty("bringToTop", NullValueHandling = NullValueHandling.Ignore)] //
        private List<string> _bringToTop = new List<string>();

        [JsonProperty("biases", NullValueHandling = NullValueHandling.Ignore)] //
        private List<Bias> _biases = new List<Bias>();
        
        [JsonProperty("numericBoosts", NullValueHandling = NullValueHandling.Ignore)] //
        private List<NumericBoost> _numericBoosts;

        [JsonProperty("influence")] private float? _influence = null;

        [JsonProperty("augmentBiases")] private bool _augmentBiases;


        public List<string> GetBringToTop()
        {
            return _bringToTop;
        }

        public Biasing SetBringToTop(List<string> bringToTop)
        {
            _bringToTop = bringToTop;
            return this;
        }

        public float? GetInfluence()
        {
            return _influence;
        }

        public Biasing SetInfluence(float influence)
        {
            _influence = influence;
            return this;
        }

        public bool IsAugmentBiases()
        {
            return _augmentBiases;
        }

        public Biasing SetAugmentBiases(bool augmentBiases)
        {
            _augmentBiases = augmentBiases;
            return this;
        }

        public List<Bias> GetBiases()
        {
            return _biases;
        }

        public Biasing SetBiases(List<Bias> biases)
        {
            _biases = biases;
            return this;
        }

        public List<NumericBoost> GetNumericBoosts()
        {
            return _numericBoosts;
        }

        public Biasing SetNumericBoosts(List<NumericBoost> numericBoosts)
        {
            _numericBoosts = numericBoosts;
            return this;
        }
    }
}