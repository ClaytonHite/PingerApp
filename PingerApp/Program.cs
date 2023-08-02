using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pinger
{
    internal class Program
    {
        static int _iterations = 4;
        static int _failures = 0;
        static List<IPStatus> _failureMessages = new List<IPStatus>();
        static List<WebsiteInfo> _websites = new List<WebsiteInfo>();
        static void Main(string[] args)
        {
            int timeOuts = 0;
            _websites = GetWebsiteList();
            foreach (WebsiteInfo website in _websites)
            {
                PingHost(website);
            }

            Console.WriteLine($"Number of Failures = {_failures}");
            foreach (var item in _failureMessages)
            {
                if (item == IPStatus.TimedOut)
                {
                    timeOuts++;
                }
                else
                {
                    Console.WriteLine(item);
                }
            }
            Console.WriteLine($"Number of TimeOuts = {timeOuts}");

            Console.WriteLine("---Website Info---");
            Console.WriteLine($"Pinged {_websites.Count} Websites.\n\n");
            int totalFails = 0;
            for (int i = 0; i < _websites.Count; i++)
            {
                if (_websites[i].Fails > 0)
                {
                    totalFails += _websites[i].Fails;
                    long totalRoundTripTime = 0;
                    long averageRoundTripTime = 0;
                    long totalTrips = _websites[i].RoundtripTimes.Count;
                    for (int j = 0; j < totalTrips; j++)
                    {
                        totalRoundTripTime += _websites[i].RoundtripTimes[j];
                    }
                    if (totalTrips != 0)
                    {
                        averageRoundTripTime = totalRoundTripTime / totalTrips;
                    }
                    Console.WriteLine($"{_websites[i].Url} Average Round Trip Time - {averageRoundTripTime} ms");
                    Console.WriteLine($"    Succeeded {_websites[i].Success} times.");
                    Console.WriteLine($"    Failed {_websites[i].Fails} times.");
                }
            }
            Console.WriteLine($"Fail Percentage = {(totalFails * 100) / (_websites.Count * _iterations)}%");
            Console.ReadLine();
        }
        private static void PingHost(WebsiteInfo website)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                for (int i = 0; i < _iterations; i++)
                {
                    pinger = new Ping();
                    PingReply reply = pinger.Send(website.Url);
                    if (reply.Status == IPStatus.Success)
                    {
                        pingable = true;
                        website.Success++;
                        website.RoundtripTimes.Add(reply.RoundtripTime);
                    }
                    else
                    {
                        _failures++;
                        website.Fails++;
                        _failureMessages.Add(reply.Status);
                    }
                    Console.WriteLine($"Pinged {website.Url}");
                    Console.WriteLine($"    Status = {reply.Status}");
                    Console.WriteLine($"    RoundTrip = {reply.RoundtripTime}");
                    Console.WriteLine($"    Pingable = {pingable}");
                    Thread.Sleep(500);
                }
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }
        }

        private static List<WebsiteInfo> GetWebsiteList()
        {
            List<string> list = new List<string>();
            list.Add("google.com");
            list.Add("youtube.com");
            list.Add("facebook.com");
            list.Add("reddit.com");
            list.Add("amazon.com");
            list.Add("twitter.com");
            list.Add("wikipedia.org");
            list.Add("instagram.com");
            list.Add("weather.com");
            list.Add("tiktok.com");
            list.Add("bing.com");
            list.Add("cnn.com");
            list.Add("twitch.tv");
            list.Add("microsoftonline.com");
            list.Add("linkedin.com");
            list.Add("walmart.com");
            list.Add("nytimes.com");
            list.Add("foxnews.com");
            list.Add("openai.com");
            list.Add("imdb.com");
            list.Add("discord.com");
            list.Add("indeed.com");
            list.Add("paypal.com");
            list.Add("espn.com");
            list.Add("office.com");
            list.Add("zoom.us");
            list.Add("apple.com");
            list.Add("zillow.com");
            list.Add("accuweather.com");
            list.Add("msn.com");
            list.Add("roblox.com");
            list.Add("homedepot.com");
            list.Add("github.com");
            list.Add("ign.com");
            list.Add("amazonaws.com");
            list.Add("dailymail.co.uk");
            list.Add("nypost.com");
            list.Add("spotify.com");
            list.Add("craigslist.org");
            list.Add("target.com");
            list.Add("bestbuy.com");
            list.Add("hulu.com");
            list.Add("adobe.com");
            list.Add("lowes.com");
            list.Add("att.com");
            list.Add("usatoday.com");
            list.Add("t-mobile.com");
            list.Add("steampowered.com");
            list.Add("washingtonpost.com");
            list.Add("tripadvisor.com");
            List<WebsiteInfo> websites = new List<WebsiteInfo>();
            foreach (var item in list)
            {
                WebsiteInfo website = new WebsiteInfo();
                website.Url = item;
                websites.Add(website);
            }
            return websites;
        }
    }

    public class WebsiteInfo
    {
        public string Url;
        public List<long> RoundtripTimes = new List<long>();
        public int Success = 0;
        public int Fails = 0;
    }
}
