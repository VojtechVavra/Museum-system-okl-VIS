using BusinessLayer.BO;
using BusinessLayer.Controllers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web
{
    public class RegisterMiddleware
    {
        private RequestDelegate next;
        public RegisterMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext ctx)
        {
            if (ctx.Request.Path.Value == "/Registrace")
            {
                // zkontrolovat dostupnost pruvodce, pokud nebude nas interni dostupny, tak se dotazat na externiho, jestli muze

                SpravaZamestnancu sz = SpravaZamestnancu.Instance;
                List<IPruvodce> pruvodce = null;

                if (ctx.Request.Method == "POST")
                {
                    string[] textBox = new string[7];

                    textBox[0] = ctx.Request.Form["jmeno"];
                    textBox[1] = ctx.Request.Form["prijmeni"];
                    textBox[2] = ctx.Request.Form["email"];
                    textBox[3] = ctx.Request.Form["pocetOsob"];
                    textBox[4] = ctx.Request.Form["pruvType"];
                    textBox[5] = ctx.Request.Form["datum"];
                    textBox[6] = ctx.Request.Form["cas"];

                    DateTime dt1= DateTime.ParseExact(textBox[5], "d/M/yyyy", CultureInfo.InvariantCulture);
                    DateTime dt2 = DateTime.ParseExact(textBox[6], "HH:mm", CultureInfo.InvariantCulture);
                    dt1 = dt1.AddHours(dt2.Hour).AddMinutes(dt2.Minute);

                    int pocet = 0;

                    // Nespravne vyplnene pole ve formulari
                    if (textBox[0] == String.Empty.ToString() || textBox[1] == String.Empty.ToString() || textBox[2] == String.Empty.ToString() || textBox[3] == String.Empty.ToString() || !textBox[2].Contains('@') || !Int32.TryParse(textBox[3], out pocet))
                    {
                        //ctx.Response.Redirect("/bird");
                        ctx.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                        await ctx.Response.WriteAsync(SpatneVyplnenaPole());
                        //return false;
                    }

                    if(textBox[4] == "2")
                    {
                        // zaslat zpravu pruvodci
                        pruvodce = sz.selectPruvodce(0);
                        ((ExterniPruvodce)pruvodce[0]).ZaslatOznameni(dt1);
                    }
                    // zapsat do sql udaje o registraci
                    Rezervace novaRezervace = new Rezervace();
                    novaRezervace.Jmeno = textBox[0];
                    novaRezervace.Prijmeni = textBox[1];
                    novaRezervace.Email = textBox[2];
                    novaRezervace.PocetOsob = pocet;
                    novaRezervace.Datum = dt1;

                    SpravaRezervaci sr = SpravaRezervaci.Instance;
                    bool uspech = sr.UlozRezervaci(novaRezervace);
                    if (uspech)
                    {
                        ctx.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                        await ctx.Response.WriteAsync(RegistraceBylaUspesna());
                    }
                    else
                    {
                        ctx.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                        await ctx.Response.WriteAsync(SpatneVyplnenaPole());
                        return;
                    }
                    
                    //return true;
                }
                else
                {
                    pruvodce = sz.selectPruvodce(1);
                    if (pruvodce != null && pruvodce.Count > 0)
                    {
                        // interni pruvodce je dostupny
                        ctx.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                        await ctx.Response.WriteAsync(FormularProRegistraci(1));
                        //return;
                    }
                    else
                    {
                        // nas interni pruvodce neni dostupny, zkontaktovat externiho, jestli muze, pokud jo, tak umoznit si zaregistrovat navstevu
                        // jinak dojde na presmerovani na stranku, ze si nelze registrovat navstevu
                        pruvodce = sz.selectPruvodce(0);

                        if (pruvodce != null && pruvodce.Count > 0)
                        {
                            // externi pruvodce je dostupny
                            ctx.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                            await ctx.Response.WriteAsync(FormularProRegistraci(2));
                        }

                        // externi pruvodce neni dostupny
                        ctx.Response.Headers.Add("Content-Type", "text/html; charset=utf-8");
                        await ctx.Response.WriteAsync(NelzeSeRezervovat());
                    }
                }
                await this.next.Invoke(ctx);
            }
            await this.next.Invoke(ctx);
        }

        string RegistraceBylaUspesna()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");

            sb.Append("<head>");
            sb.Append("<title>Registrace byla úspěšná</title>");
            sb.Append("</head>");

            sb.Append("<body>");
            sb.Append("<h1>Registrace byla úspěšná</h1>");

            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        string FormularProRegistraci(int typPruvodce)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");

            sb.Append("<head>");
            sb.Append("<title>Registrace</title>");
            sb.Append("</head>");

            sb.Append("<body>");
            sb.Append("<h1>Formulář pro rezervace návštěvi s průvodcem</h1>");

            sb.Append("<form method = \"post\" action = \"/Registrace\" id = \"form1\" >");
            sb.Append("<label for= \"fname\">Jméno:</label><br>");
            sb.Append("<input type = 'text' name = 'jmeno'/><br>");

            sb.Append("<label for= \"fname\">Příjmení:</label><br>");
            sb.Append("<input type = 'text' name = 'prijmeni' /><br>");

            sb.Append("<label for= \"fname\">Email:</label><br>");
            sb.Append("<input type = 'text' name = 'email' /><br>");

            sb.Append("<label for= \"fpocet\">Počet osob:</label><br>");
            sb.Append("<input type = 'text' name = 'pocetOsob' /><br>");

            sb.Append("<input type = \"hidden\" id = \"pruvType\" name = \"pruvType\" value = \"" + typPruvodce + "\">");

            DateTime d1 = new DateTime();
            d1 = DateTime.Now;
            sb.Append("<br><label for= \"fdatum\">Vyberte datum:</label>");
            sb.Append("<select name = 'datum' id = \"datum\" >");
            for (int i = 0; i < 7; i++)
            {
                //String s = String.Format("{0:d}-{0:M}-{0:yyyy}", d1);
                String s = d1.ToString("d/M/yyyy", CultureInfo.InvariantCulture);
                //sb.Append("<option value = \"volvo\" > Volvo </option>");
                sb.Append("<option value = \"" + s + "\">" + s + "</option>");
                d1 = d1.AddDays(1);
            }

            sb.Append("</select>");
            sb.Append("<br><br>");

            DateTime d2 = new DateTime(2020, 1, 1, 9, 0, 0);
            //DateTime d2 = d2;

            sb.Append("<label for= \"cas\">Vyberte čas:</label>");
            sb.Append("<select name = \"cas\" id = \"cas\" >");
            for(int i = 0; i < 15; i++)
            {
                String s = String.Format("{0:HH}:{0:mm}", d2);
                sb.Append("<option value = \"" + s + "\">" + s + "</option>");
                d2 = d2.AddMinutes(30);
            }

            sb.Append("</select>");
            sb.Append("<br><br>");

            sb.Append("<input type = \"submit\" value = 'Submit' /><br>");
            sb.Append("</form>");

            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        string SpatneVyplnenaPole()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");

            sb.Append("<head>");
            sb.Append("<title>Registrace</title>");
            sb.Append("</head>");

            sb.Append("<body>");
            sb.Append("<h1>Formular NEBYL odeslan</h1>");
            sb.Append("<p>Neplantá, nebo nevyplněná pole</p>");

            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }

        string NelzeSeRezervovat()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");

            sb.Append("<head>");
            sb.Append("<title>Chyba registrace</title>");
            sb.Append("</head>");

            sb.Append("<body>");
            sb.Append("<h1>Momentálně nelze provádět registrace</h1>");
            sb.Append("<h1>Zkuste to o pár dnů později.</h1>");
            sb.Append("<h1>Děkujeme za pochopení.</h1>");

            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}
