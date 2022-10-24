using System;

namespace Mz.Numerics
{
    public class RandomMarsenneTwister
    {
        public RandomMarsenneTwister(long seed = -1)
        {
            if (seed < 0) {
                seed = DateTime.Now.Ticks;
            }

            /* Period parameters */
            _n = 624;
            _m = 397;
            _matrixA = 0x9908b0df;   /* constant vector a */
            _maskUpper = 0x80000000; /* most significant w-r bits */
            _maskLower = 0x7fffffff; /* least significant r bits */

            _mt = new uint[_n]; /* the array for the state vector */
            _mti = _n + 1; /* mti == N+1 means mt[N] is not initialized */

            _InitializeSeed((uint)seed);
        }

        private uint _n;
        private uint _m;
        private uint _matrixA;
        private uint[] _mt;
        private uint _mti;
        private uint _maskUpper;
        private uint _maskLower;

        private void _InitializeSeed(uint seed)
        {
            _mt[0] = seed;
            for (_mti=1; _mti<_n; _mti++) {
                var s = _mt[_mti-1] ^ (_mt[_mti-1] >> 30);
                _mt[_mti] = ((((s & 0xffff0000) >> 16) * 1812433253) << 16) + (s & 0x0000ffff) * 1812433253 + _mti;
                // See Knuth TAOCP Vol2. 3rd Ed. P.106 for multiplier.
                // In the previous versions, MSBs of the seed affect   
                // only MSBs of the array mt[].                        
                // 2002/01/09 modified by Makoto Matsumoto             
                _mt[_mti] >>= 0;
                // for >32 bit  machines
            }
        }

        public uint RandomUnsignedInt()
        {
            uint y;
            var mag01 = new uint[] { 0x0, _matrixA };
      
            /* mag01[x] = x * _matrixA  for x = 0,1 */

            if (_mti >= _n) { /* generate N words at one time */
                uint kk;

                if (_mti == _n + 1)  /* if init_seed() has not been called, */
                    _InitializeSeed(5489);  /* a default initial seed is used */

                for (kk = 0; kk < _n - _m; kk++) {
                    y = (_mt[kk] & _maskUpper) | (_mt[kk + 1] & _maskLower);
                    _mt[kk] = _mt[kk+_m] ^ (y >> 1) ^ mag01[y & 0x1];
                }
                for (;kk < _n - 1; kk++) {
                    y = (_mt[kk] & _maskUpper) | (_mt[kk + 1] & _maskLower);
                    _mt[kk] = _mt[kk + (_m - _n)] ^ (y >> 1) ^ mag01[y & 0x1];
                }
                y = (_mt[_n - 1] & _maskUpper) | (_mt[0] & _maskLower);
                _mt[_n - 1] = _mt[_m - 1] ^ (y >> 1) ^ mag01[y & 0x1];

                _mti = 0;
            }

            y = _mt[_mti++];

            /* Tempering */
            y ^= (y >> 11);
            y ^= (y << 7) & 0x9d2c5680;
            y ^= (y << 15) & 0xefc60000;
            y ^= (y >> 18);

            return y;
        }

        /// <summary>
        /// generates a random number on [0,1)-real-interval
        /// </summary>
        public float Random0To1()
        {
            return RandomUnsignedInt() * (1.0f / 4294967296.0f);
            /* divided by 2^32 */
        }
    }
}