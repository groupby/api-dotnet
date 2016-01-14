using System.Collections.Generic;

namespace GroupByInc.Api.Requests
{
    public class Biasing
    {
        private List<string> _bringToTop;

        private float? _influence;

        private bool _augmentBiases;

        private List<Bias> _biases;

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
    }
}