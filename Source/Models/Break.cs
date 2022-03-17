using System;
using System.Runtime.Serialization;
using System.Text;

namespace BingMapsRESTToolkit
{
    /// <summary>
    /// A break period 
    /// </summary>
    [DataContract]
    public class Break
    {
        /// <summary>
        /// The start of the time window for a break.
        /// </summary>
        [DataMember(Name = "startTime")]
        internal string StartTime { get; set; }

        /// <summary>
        /// The start of the time window for a break.
        /// </summary>
        public DateTime StartTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(StartTime))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.GetDateTimeFromUTCString(StartTime);
                }
            }
            set
            {
                if (value == null)
                {
                    StartTime = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.GetUTCString(value);

                    if (v != null)
                    {
                        StartTime = v;
                    }
                    else
                    {
                        StartTime = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// The end of the time window for a break.
        /// </summary>
        [DataMember(Name = "endTime")]
        internal string EndTime { get; set; }

        /// <summary>
        /// The end of the time window for a break.
        /// </summary>
        public DateTime EndTimeUtc
        {
            get
            {
                if (string.IsNullOrEmpty(EndTime))
                {
                    return DateTime.Now;
                }
                else
                {
                    return DateTimeHelper.GetDateTimeFromUTCString(EndTime);
                }
            }
            set
            {
                if (value == null)
                {
                    EndTime = string.Empty;
                }
                else
                {
                    var v = DateTimeHelper.GetUTCString(value);

                    if (v != null)
                    {
                        EndTime = v;
                    }
                    else
                    {
                        EndTime = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// The duration of the break period within the time window. If not specified, all time between start and end time will be used as the duration.
        /// </summary>
        [DataMember(Name = "duration")]
        public string Duration { get; set; }

        /// <summary>
        /// The duration of the break period within the time window. If not specified, all time between start and end time will be used as the duration.
        /// </summary>
        public TimeSpan? DurationTimeSpan
        {
            get
            {
                if (TimeSpan.TryParse(Duration, out TimeSpan ts))
                {
                    return ts;
                }

                return null;
            }
            set
            {
                if (value == null)
                {
                    Duration = string.Empty;
                }
                else
                {
                    Duration = string.Format("{0:g}",value);
                }
            }
        }

        public override string ToString()
        {
            if(StartTimeUtc == null)
            {
                throw new Exception("Start time must be specified for break.");
            }

            if (EndTimeUtc == null)
            {
                throw new Exception("End time must be specified for break.");
            }

            var sb = new StringBuilder("{");

            sb.AppendFormat("\"startTime\":\"{0}\",", StartTime);
            sb.AppendFormat("\"endTime\":\"{0}\",", EndTime);

            if (DurationTimeSpan == null || !DurationTimeSpan.HasValue)
            {
                DurationTimeSpan = EndTimeUtc - StartTimeUtc;
            }

            //https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings
            sb.AppendFormat("\"duration\":\"{0}\"", Duration);

            sb.Append("}");

            return sb.ToString();
        }
    }
}
