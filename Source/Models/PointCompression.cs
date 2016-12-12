/*
 * Copyright(c) 2017 Microsoft Corporation. All rights reserved. 
 * 
 * This code is licensed under the MIT License (MIT). 
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal 
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
 * of the Software, and to permit persons to whom the Software is furnished to do 
 * so, subject to the following conditions: 
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software. 
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE. 
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// This compression algorithm encodes/decodes a collections of coordinates into a string. 
    /// This algorithm is used for generating a compressed collection of coordinates for use 
    /// with the Bing Maps REST Elevation Service. This algorithm is also used for decoding 
    /// the compressed coordinates returned by the GeoData API.
    /// 
    /// These algorithms come from the following documentation:
    /// http://msdn.microsoft.com/en-us/library/jj158958.aspx
    /// http://msdn.microsoft.com/en-us/library/dn306801.aspx
    /// </summary>
    public static class PointCompression
    {
        #region Private Properties

        private const string LookUpTable = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_-";

        #endregion

        #region Public Methods

        /// <summary>
        /// Compresses a list of coordinates into a string.
        /// Based on: http://msdn.microsoft.com/en-us/library/jj158958.aspx
        /// </summary>
        /// <param name="points">Collection of coordinates to compress.</param>
        /// <returns>A compressed string representing a collection coordinates.</returns>
        public static string Encode(List<Coordinate> points)
        {
            long latitude = 0;
            long longitude = 0;
            StringBuilder sb = new StringBuilder();

            foreach (var point in points)
            {
                // step 2
                var newLatitude = (long)Math.Round(point.Latitude * 100000);
                var newLongitude = (long)Math.Round(point.Longitude * 100000);

                // step 3
                long dy = newLatitude - latitude;
                long dx = newLongitude - longitude;
                latitude = newLatitude;
                longitude = newLongitude;

                // step 4 and 5
                dy = (dy << 1) ^ (dy >> 31);
                dx = (dx << 1) ^ (dx >> 31);

                // step 6
                long index = ((dy + dx) * (dy + dx + 1) / 2) + dy;

                while (index > 0)
                {
                    // step 7
                    long rem = index & 31;
                    index = (index - rem) / 32;

                    // step 8
                    if (index > 0)
                    {
                        rem += 32;
                    }

                    // step 9
                    sb.Append(LookUpTable[(int)rem]);
                }
            }

            // step 10
            return sb.ToString();
        }

        /// <summary>
        /// Decodes a collection of coordinates from a compressed string.
        /// Based on: http://msdn.microsoft.com/en-us/library/dn306801.aspx
        /// </summary>
        /// <param name="value">Compressed string to decode</param>
        /// <param name="parsedValue">Collection of decoded coordinates.</param>
        /// <returns>A boolean indicating if the algorithm was able to decode the compressed coordinates or not.</returns>
        public static bool TryDecode(string value, out List<Coordinate> parsedValue)
        {
            parsedValue = null;
            var list = new List<Coordinate>();
            int index = 0;
            int xsum = 0, ysum = 0;

            while (index < value.Length)        // While we have more data,
            {
                long n = 0;                     // initialize the accumulator
                int k = 0;                      // initialize the count of bits

                while (true)
                {
                    if (index >= value.Length)  // If we ran out of data mid-number
                        return false;           // indicate failure.

                    int b = LookUpTable.IndexOf(value[index++]);

                    if (b == -1)                // If the character wasn't on the valid list,
                        return false;           // indicate failure.

                    n |= ((long)b & 31) << k;   // mask off the top bit and append the rest to the accumulator
                    k += 5;                     // move to the next position
                    if (b < 32) break;          // If the top bit was not set, we're done with this number.
                }

                // The resulting number encodes an x, y pair in the following way:
                //
                //  ^ Y
                //  |
                //  14
                //  9 13
                //  5 8 12
                //  2 4 7 11
                //  0 1 3 6 10 ---> X

                // determine which diagonal it's on
                int diagonal = (int)((Math.Sqrt(8 * n + 5) - 1) / 2);

                // subtract the total number of points from lower diagonals
                n -= diagonal * (diagonal + 1L) / 2;

                // get the X and Y from what's left over
                int ny = (int)n;
                int nx = diagonal - ny;

                // undo the sign encoding
                nx = (nx >> 1) ^ -(nx & 1);
                ny = (ny >> 1) ^ -(ny & 1);

                // undo the delta encoding
                xsum += nx;
                ysum += ny;

                double lat = ysum * 0.00001;
                double lon = xsum * 0.00001;

                //Trim latlong values to supported ranges
                lat = Math.Max(-85, Math.Min(85, lat));
                lon = Math.Max(-180, Math.Min(180, lon));

                list.Add(new Coordinate(lat, lon));
            }

            parsedValue = list;

            return true;
        }

        #endregion
    }
}