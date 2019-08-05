using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ColorQuery
{
    public class Api
    {
        //https://www.colourlovers.com/api#palettes
        private static readonly Uri ApiBase =
            new Uri("http://www.colourlovers.com/api/");


        public async Task<Palette> GetPalette(int paletteId)
        {
            var client = new HttpClient();
            client.BaseAddress = ApiBase;

            var resp = await client.GetAsync($"palette/{paletteId}?format=json");
            var ps = JsonConvert.DeserializeObject<List<Palette>>(
                    await resp.Content.ReadAsStringAsync());
            if(ps.Count == 0)
            {
                throw new ArgumentException($"Palette {paletteId} does not exist.");
            }

            return ps[0];
        }

        public async Task<List<Palette>> SearchPalettes(SearchVerb c)
        {
            var sb = new StringBuilder();
            sb.Append("?format=json&");
            if(c.Hues != null)
            {
                sb.Append("hueOption=");
                sb.Append(String.Join(',', c.Hues.Distinct().Select(h => h.HueText)));
                sb.Append('&');
            }
            if(c.HexCodes != null)
            {
                sb.Append("hex=");
                sb.Append(String.Join(',', c.HexCodes.Select(h => 
                    h.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                        ? h.Substring(2).ToUpperInvariant()
                        : h.ToUpperInvariant())));
                sb.Append('&');
            }
            if(c.Keywords != null)
            {
                sb.Append("keywords=");
                sb.Append(String.Join('+', c.Keywords.Select(k =>
                    Uri.EscapeDataString(k).Replace("%20", "+"))));
                sb.Append('&');
            }
            sb.Remove(sb.Length-1, 1);

            var client = new HttpClient();
            client.BaseAddress = ApiBase;
            var resp = await client.GetAsync("palettes/" + c.SortType + sb.ToString());
            return JsonConvert.DeserializeObject<List<Palette>>(
                    await resp.Content.ReadAsStringAsync());
        }
    }
}