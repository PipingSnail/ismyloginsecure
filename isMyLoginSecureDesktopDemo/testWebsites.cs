// Implementation: Stephen Kellett 28 December 2017..10 January 2018 and March/April 2025
// Copyright (c) Software Verify, IsMyLoginSecure 2017-2025.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
// and associated documentation files (the “Software”), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, 
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is 
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN 
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// The above licence is the MIT Licence. https://opensource.org/license/MIT

namespace isMyLoginSecureDesktopDemo
{
    class testWebsites
    {
        /// <summary>
        /// Simple struct to hold two items of information together - a business name and a website.
        /// </summary>
        /// <remarks>Arrays of this structure will be used to setup lists of institutions for automatic testing.</remarks>
        public struct websiteInfo
        {
            public websiteInfo(string name,
                               string website)
            {
                businessName = name;
                url = website;
            }

            public string businessName { get; }
            public string url { get; }
        }

        // some test websites

        public static readonly websiteInfo[] theTestWebsites =
        {
            new websiteInfo("Liam Blizard",         "liamblizard.co.uk/"),
            new websiteInfo("Is My Login Secure",   "www.ismyloginsecure.com"),
            new websiteInfo("Software Verify",      "www.softwareverify.com"),
            new websiteInfo("First Direct",         "www1.firstdirect.com/1/2/"),
            new websiteInfo("Bank of Ireland",      "bankofirelanduk.com/")
        };

        /// <summary>
        /// Get the list of test websites
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfTestWebsites()
        {
            return theTestWebsites;
        }

        // stock traders

        private static readonly websiteInfo[] listOfStockTraders = 
        {
            new websiteInfo("Alliance Trust Saving",        "atonline.alliancetrust.co.uk/atonline/login.jsp"),
            new websiteInfo("ANZ",                          "shareinvesting.anz.com/home.aspx"),
            new websiteInfo("Angel Broking",                "www.angelbroking.com/online-share-trading"),
            new websiteInfo("Bank of Scotland",             "www.bankofscotland.co.uk/"),
            new websiteInfo("Barclays",                     "www.smartinvestor.barclays.co.uk/campaign/investment-account.html"),
            new websiteInfo("Barclays Trading Hub",         "www.barclaystradinghub.co.uk/home/what-is-cfd-trading/spread-trading-versus-contracts-for-difference.html"),
            new websiteInfo("Beaufort Securities",          "www.beaufortsecurities.com/online-share-dealing-t-14"),
            new websiteInfo("Belforfx",                     "bonus.belforfx.com"),
            new websiteInfo("Broker Direct",                "www.brokerdirect.co.uk/News/ShareTradingNew.aspx"),
            new websiteInfo("Charles Schwab",               "www.schwab.co.uk/public/schwab-uk-en/us-investing"),
            new websiteInfo("Charles Stanley Direct",       "www.charles-stanley-direct.co.uk"),
            new websiteInfo("Citi",                         "www.citibank.co.uk/personal/equities.do"),
            new websiteInfo("City Index",                   "www.cityindex.co.uk/share-trading/"),
            new websiteInfo("CMC Markets",                  "www.cmcmarkets.com/en-au/markets-shares"),
            new websiteInfo("Computershare",                "www.computershare.trade/"),
            new websiteInfo("Degiro",                       "www.degiro.co.uk/"),
            new websiteInfo("Digital Look",                 "www.digitallook.com"),
            new websiteInfo("Direct Market Touch",          "www.directmarkettouch.com/"),
            new websiteInfo("Etoro",                        "www.etoro.com/"),
            new websiteInfo("Easy Share Trading",           "easysharetrading.co.uk/stocks-and-shares-courses/"),
            new websiteInfo("ETrade",                       "us.etrade.com/home"),
            new websiteInfo("ETX Capital",                  "www.etxcapital.co.uk/equities-trading"),
            new websiteInfo("Equiniti share view",          "www.shareview.co.uk/4/Info/Portfolio/Default/en/Home/Pages/Home.aspx"),
            new websiteInfo("Fair Investment Company",      "www.fairinvestment.co.uk/uk_share_trading.aspx"),
            new websiteInfo("Fantasy Stock Exchange",       "www.fantasystockexchange.biz/"),
            new websiteInfo("FCMB Group Plc",               "fcmbgroup.com/share-trading-policy"),
            new websiteInfo("First Direct",                 "www1.firstdirect.com/1/2/savings-and-investments/sharedealing"),
            new websiteInfo("Fortrade",                     "www.fortrade.com/"),
            new websiteInfo("Free Trade",                   "freetrade.io/"),
            new websiteInfo("FxPro",                        "www.fxpro.co.uk/trading/shares"),
            new websiteInfo("Get Stocks",                   "getstocks.com"),
            new websiteInfo("Halifax",                      "www.halifax.co.uk/sharedealing/our-accounts/share-dealing-account/Default.asp"),
            new websiteInfo("Hargreaves Lansdown",          "www.hl.co.uk/investment-services/fund-and-share-account"),
            new websiteInfo("HSBC",                         "investments.hsbc.co.uk/product/9/sharedealing"),
            new websiteInfo("IG",                           "www.ig.com/uk/shares"),
            new websiteInfo("Interactive investor",         "www.iii.co.uk/"),
            new websiteInfo("Internaxx",                    "www.internaxx.com/"),
            new websiteInfo("iDealing",                     "www.idealing.com/en/index"),
            new websiteInfo("iWeb",                         "www.iweb-sharedealing.co.uk/share-dealing-home.asp"),
            new websiteInfo("Lloyds Bank",                  "www.lloydsbank.com/share-dealing/share-dealing-account.asp"),
            new websiteInfo("London Capital Group",         "www.lcg.com/uk/"),
            new websiteInfo("London South East",            "www.lse.co.uk/share-trading/"),
            new websiteInfo("Natwest Invest",               "personal.natwest.com/personal/investments/natwest_invest/natwest-invest.html"),
            new websiteInfo("Plus 500",                     "www.plus500.co.uk/Trading/Stocks"),
            new websiteInfo("Redmayne Bentley",             "www.redmayne.co.uk/stockbroking"),
            new websiteInfo("Religare broking",             "www.religareonline.com/"),
            new websiteInfo("RHB Trade Smart",              "rhbtradesmart.com/"),
            new websiteInfo("Saga share direct",            "www.sagasharedirect.co.uk/"),
            new websiteInfo("Saxo Capital Markets",         "www.home.saxo/en-gb"),
            new websiteInfo("Self Trade ",                  "selftrade.co.uk/"),
            new websiteInfo("Shareprices.com",              "shareprices.com/trading/"),
            new websiteInfo("Share Scope",                  "www.sharescope.co.uk/"),
            new websiteInfo("Stock Trade",                  "www.stocktrade.co.uk/"),
            new websiteInfo("Sure Trader",                  "www.suretrader.com/"),
            new websiteInfo("SVS XO",                       "svsxo.com/"),
            new websiteInfo("The share centre",             "www.share.com/share-account/"),
            new websiteInfo("Westpac",                      "www.westpac.com.au/personal-banking/investments/share-trading/"),
            new websiteInfo("UAEXChange",                   "www.uaeexchange-etrade.com/"),
            new websiteInfo("UK Trading View",              "uk.tradingview.com/"),
            new websiteInfo("Virgin Money",                 "uk.virginmoney.com/virgin/isa/stocks-and-shares/#"),
            new websiteInfo("Which Way To Pay",             "www.whichwaytopay.com/compare-share-dealing-summary.asp"),
            new websiteInfo("XM",                           "www.xm.co.uk/"),
            new websiteInfo("XO",                           "www.x-o.co.uk/"),
            new websiteInfo("XTB",                          "www.xtb.com/en"),
            new websiteInfo("Yorkshire Building Society",   "sharedealing.ybs.co.uk/"),
            new websiteInfo("You Invest",                   "www.youinvest.co.uk/dealing-account"),
        };

        /// <summary>
        /// Get the list of Stock Traders
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfStockTraders()
        {
            return listOfStockTraders;
        }

        // insurance companies

        private static readonly websiteInfo[] listOfInsuranceCompanies =
        {
            new websiteInfo("AEGON UK",                         "www.aegon.co.uk/index.html"),
            new websiteInfo("AXA",                              "www.axa.co.uk/home.aspx"),
            new websiteInfo("Allianz SE",                       "www.allianz.com/en/"),
            new websiteInfo("Aviva",                            "www.aviva.co.uk/"),
            new websiteInfo("Direct Line Insurance",            "www.directline.com/"),
            new websiteInfo("FM Global",                        "www.fmglobal.com/"),
            new websiteInfo("Hiscox",                           "www.hiscox.co.uk/"),
            new websiteInfo("Legal & General",                  "www.legalandgeneral.com/insurance/"),
            new websiteInfo("NFU Mutual",                       "www.nfumutual.co.uk/"),
            new websiteInfo("Old Mutual",                       "www.oldmutualplc.com/"),
            new websiteInfo("Phoenix",                          "www.phoenixlife.co.uk/"),
            new websiteInfo("Prudential",                       "www.prudential.co.uk/"),
            new websiteInfo("QBE Insurance",                    "www.group.qbe.com/"),
            new websiteInfo("Royal London Asset Management",    "www.rlam.co.uk/"),
            new websiteInfo("Royal London Group",               "www.royallondon.com/"),
            new websiteInfo("RSA Insurance Group",              "www.rsagroup.com/"),
            new websiteInfo("Standard Life",                    "www.standardlife.com/dotcom/index.page"),
            new websiteInfo("Southern Rock Insurance",          "www.sricl.com/"),
            new websiteInfo("XL Group",                         "xlgroup.com/"),
            new websiteInfo("Zurich Insurance",                 "www.zurich.co.uk/"),
        };

        /// <summary>
        /// Get the list of Insurance Companies
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfInsuranceCompanies()
        {
            return listOfInsuranceCompanies;
        }

        // pension funds

        private static readonly websiteInfo[] listOfPensionFunds = 
        {
            new websiteInfo("Aviva Staff Pension Scheme",               "www.avivastaffpensions.co.uk/retired/default.aspx"),
            new websiteInfo("BAE Systems Pension Scheme",               "www.baesystemspensions.com/"),
            new websiteInfo("Barclays Bank UK Retirement Fund",         "epa.towerswatson.com/accounts/barclays/"),
            new websiteInfo("BBC Pension Trust Ltd",                    "www.bbc.co.uk/mypension/join"),
            new websiteInfo("BP Pension Fund",                          "pensionline.bp.com/Homepage"),
            new websiteInfo("British Airways Pension Scheme",           "www.mybapension.com/"),
            new websiteInfo("British Coal Staff Superannuation Scheme", "www.bcsss-pension.org.uk/"),
            new websiteInfo("British Steel Pension Scheme",             "www.bspensions.com/"),
            new websiteInfo("BT Pension Scheme",                        "www.btpensions.net/"),
            new websiteInfo("Co-operative Group Pension Scheme(Pace)",  "pensions.coop.co.uk/"),
            new websiteInfo("Electricity Supply Pension Scheme",        "megtpensions.com/contact-us/"),
            new websiteInfo("Greater Manchester Pension Fund",          "www.gmpf.org.uk/"),
            new websiteInfo("HBOS Final Salary Pension Scheme",         "www.lloydsbankinggrouppensions.com/"),
            new websiteInfo("HSBC Bank UK Pension Scheme",              "www.futurefocus.staff.hsbc.co.uk/"),
            new websiteInfo("ICI Pension Fund",                         "www.icipensionfund.org.uk/"),
            new websiteInfo("Lloyds TSB Group Pension Scheme",          "www.lloydsbankinggrouppensions.com/"),
            new websiteInfo("Mineworkers Pension Scheme",               "www.mps-pension.org.uk/"),
            new websiteInfo("National Grid UK Pension Scheme",          "www.nationalgridpensions.com/362/1320/welcome-to-the-national-grid-uk-pension-scheme-website"),
            new websiteInfo("Railways Pension Scheme",                  "www.railwayspensions.co.uk/"),
            new websiteInfo("RBS Group Pension Fund ",                  "rbs.tbs.aon.com/"),
            new websiteInfo("RBS Group Pensioner’s Association",        "rbsgpa.org.uk/"),
            new websiteInfo("Rolls-Royce Pension Fund",                 "www.rolls-roycepensions.com/Homepage"),
            new websiteInfo("Royal Mail Pension Plan",                  "www.royalmailpensionplan.co.uk/"),
            new websiteInfo("Shell Contributory Pension Fund",          "pensions.shell.co.uk/scpf.html"),
            new websiteInfo("Strathclyde Pension Fund",                 "www.spfo.org.uk/"),
            new websiteInfo("Universities Superannuation Scheme",       "www.uss.co.uk/"),
            new websiteInfo("West Midlands Pension Fund",               "www.wmpfonline.com/"),
            new websiteInfo("West Yorkshire Pension Scheme",            "www.wypf.org.uk/"),
        };

        /// <summary>
        /// Get the list of Pension Funds
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfPensionFunds()
        {
            return listOfPensionFunds;
        }

        // healthcare companies

        private static readonly websiteInfo[] listOfHealthcareCompanies = 
        {
            new websiteInfo("Aviva Healthcare",                     "www.aviva.co.uk/"),
            new websiteInfo("AXA PPP",                              "www.axappphealthcare.co.uk/"),
            new websiteInfo("Benenden Healthcare Society",          "www.benenden.co.uk/"),
            new websiteInfo("Birmingham Hospital Saturday Fund",    "www.bhsf.co.uk/"),
            new websiteInfo("Bupa",                                 "www.bupa.co.uk/"),
            new websiteInfo("CS Healthcare",                        "www.cshealthcare.co.uk/"),
            new websiteInfo("Engage Mutual Assurance",              "www.onefamily.com/"),
            new websiteInfo("Exeter Family Friendly",               "www.the-exeter.com/"),
            new websiteInfo("General & Medical Healthcare",         "www.generalandmedical.com/"),
            new websiteInfo("Health-on-Line",                       "www.health-on-line.co.uk/"),
            new websiteInfo("Healthshield",                         "www.healthshield.co.uk/"),
            new websiteInfo("HSF",                                  "www.hsf.co.uk"),
            new websiteInfo("Insurety",                             "www.april-uk.com"),
            new websiteInfo("Medicash",                             "www.medicash.org"),
            new websiteInfo("National Friendly",                    "nationalfriendly.co.uk/"),
            new websiteInfo("Saga",                                 "www.saga.co.uk"),
            new websiteInfo("Secure Health",                        "www.securehealth.co.uk/"),
            new websiteInfo("Sovereign Health",                     "www.sovereignhealthcare.co.uk/"),
            new websiteInfo("Simply Health",                        "www.simplyhealth.co.uk/"),
            new websiteInfo("Vitality",                             "www.vitality.co.uk/"),
            new websiteInfo("Westfield",                            "www.westfieldhealth.com/"),
            new websiteInfo("WHA",                                  "www.whahealthcare.co.uk/"),
            new websiteInfo("WHCA",                                 "www.orchardhealthcare.co.uk/"),
            new websiteInfo("WPA",                                  "www.wpa.org.uk/"),
        };

        /// <summary>
        /// Get the list of Healthcare Companies
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfHealthcareCompanies()
        {
            return listOfHealthcareCompanies;
        }

        // ecommerce companies

        private static readonly websiteInfo[] listOfEcommerceCompanies = 
        {
            new websiteInfo("2C2P",                         "www.2c2p.com/"),
            new websiteInfo("Adyen",                        "www.adyen.com/"),
            new websiteInfo("Alipay",                       "intl.alipay.com/"),
            new websiteInfo("Amazon Pay",                   "pay.amazon.com/uk"),
            new websiteInfo("Apple Pay",                    "www.apple.com/uk/apple-pay/"),
            new websiteInfo("Atos",                         "atos.net/en-gb/united-kingdom"),
            new websiteInfo("Authorize.Net",                "www.authorize.net/"),
            new websiteInfo("Bambora",                      "www.bambora.com/sv/overview/#market-select"),
            new websiteInfo("BitPay",                       "bitpay.com/"),
            new websiteInfo("BPAY",                         "www.bpay.co.uk/"),
            new websiteInfo("Braintree",                    "www.braintreepayments.com/en-gb"),
            new websiteInfo("CM Telecom",                   "www.cm.com/"),
            new websiteInfo("Creditcall",                   "www.creditcall.com/"),
            new websiteInfo("CyberSource",                  "www.cybersource.com"),
            new websiteInfo("DigiCash",                     "www.digi.cash/"),
            new websiteInfo("Digital River",                "www.digitalriver.com/"),
            new websiteInfo("Dwolla",                       "www.dwolla.com/"),
            new websiteInfo("Elavon",                       "www.elavon.co.uk/index.html"),
            new websiteInfo("Euronet Worldwide",            "www.euronetworldwide.com/"),
            new websiteInfo("eWAY",                         "eway.io/uk"),
            new websiteInfo("First Data",                   "www.firstdata.com/en_gb/home.html"),
            new websiteInfo("Flooz",                        "www.flooz.me/"),
            new websiteInfo("Fortumo Online",               "fortumo.com/"),
            new websiteInfo("GoCardless",                   "gocardless.com/"),
            new websiteInfo("Heartland Payment Systems",    "www.heartlandpaymentsystems.com/about-us"),
            new websiteInfo("Ingenico",                     "www.ingenico.com/"),
            new websiteInfo("Klarna",                       "www.klarna.com/uk/"),
            new websiteInfo("ModusLink",                    "www.moduslink.com/"),
            new websiteInfo("MPay",                         "www.mpay.al/en"),
            new websiteInfo("Neteller",                     "www.neteller.com/en/"),
            new websiteInfo("Nochex",                       "www.nochex.com/gb/"),
            new websiteInfo("OFX",                          "www.ofx.com/en-gb/"),
            new websiteInfo("PagSeguro",                    "pagseguro.uol.com.br/"),
            new websiteInfo("PayPal",                       "www.paypal.com/uk/home"),
            new websiteInfo("Payoneer",                     "www.payoneer.com/main/"),
            new websiteInfo("Paymentwall",                  "www.paymentwall.com/en/"),
            new websiteInfo("PayPoint",                     "www.paypoint.com/en-gb/consumers/store-locator"),
            new websiteInfo("Paysbuy",                      "www.paysbuy.com/"),
            new websiteInfo("Paysafe Group",                "www.paysafe.com/"),
            new websiteInfo("PayStand",                     "www.paystand.com/"),
            new websiteInfo("Payzone",                      "www.payzone.co.uk/"),
            new websiteInfo("Qiwi",                         "qiwi.com/"),
            new websiteInfo("Realex Payments",              "www.realexpayments.com/uk/"),
            new websiteInfo("Red Dot Payment",              "reddotpayment.com/"),
            new websiteInfo("Sage Group ",                  "www.sage.com/en-gb/"),
            new websiteInfo("Skrill",                       "www.skrill.com/en/"),
            new websiteInfo("Stripe",                       "stripe.com/gb"),
            new websiteInfo("Square",                       "squareup.com/gb"),
            new websiteInfo("SWREG",                        "faq.swreg.org/"),
            new websiteInfo("Tencent",                      "www.tencent.com/en-us/"),
            new websiteInfo("TIMWE",                        "www.timwe.com/"),
            new websiteInfo("TransferWise",                 "transferwise.com/"),
            new websiteInfo("True Money",                   "www.truemoney.com/"),
            new websiteInfo("Trustly Online",               "trustly.com/en/"),
            new websiteInfo("Verifone",                     "www.verifone.co.uk/"),
            new websiteInfo("WebMoney",                     "www.wmtransfer.com/"),
            new websiteInfo("WeChat Pay",                   "pay.weixin.qq.com/index.php/public/wechatpay"),
            new websiteInfo("WePay",                        "go.wepay.com/"),
            new websiteInfo("Wirecard",                     "www.wirecard.com/"),
            new websiteInfo("Worldpay",                     "www.worldpay.com"),
            new websiteInfo("Xendpay",                      "www.xendpay.com/"),
            new websiteInfo("Xsolla",                       "www.xsolla.com/"),
            new websiteInfo("Yandex.Money",                 "money.yandex.ru/"),
        };

        /// <summary>
        /// Get the list of Ecommerce Companies
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfEcommerceCompanies()
        {
            return listOfEcommerceCompanies;
        }

        // casinos

        private static readonly websiteInfo[] listOfCasinos = 
        {
            new websiteInfo("21Jackpots",               "21jackpots.com/"),
            new websiteInfo("32Red Casino",             "www.32red.com/"),
            new websiteInfo("50 Stars Casino",          "www.50starscasino.com/english/eur/download.html"),
            new websiteInfo("888Casino",                "www.888casino.com/"),
            new websiteInfo("All British Casino",       "www.allbritishcasino.com/"),
            new websiteInfo("All Irish Casino",         "www.allirishcasino.com/"),
            new websiteInfo("BETAT Casino",             "betat.co.uk/home/"),
            new websiteInfo("Betfred Casino",           "www.betfred.com/casino"),
            new websiteInfo("Betsafe Casino",           "www.betsafe.com/en/casino"),
            new websiteInfo("Betspin Casino",           "www.betspin.com/gb"),
            new websiteInfo("Betway Casino",            "casino.betway.com/lobby/en/#/home"),
            new websiteInfo("Bet-At-Home Casino",       "uk.bet-at-home.com/"),
            new websiteInfo("bgo Vegas",                "www.bgo.com/"),
            new websiteInfo("Cashmio Casino",           "www.cashmio.com/en"),
            new websiteInfo("CasinoLuck",               "www.casinoluck.com/"),
            new websiteInfo("Casino Kings",             "www.casinokings.com/"),
            new websiteInfo("Casino Magix",             "www.casinomagix.com/"),
            new websiteInfo("Casumo Casino",            "www.casumo.com/en-gb/"),
            new websiteInfo("ComeOn Casino",            "www.comeon.com/"),
            new websiteInfo("Carnival Casino",          "www.carnivalcasino.com/"),
            new websiteInfo("Casino Cruise",            "www.casinocruise.com/en-gb"),
            new websiteInfo("Casino King",              "www.casinokings.com/"),
            new websiteInfo("Casino Plex",              "casinoplex.co.uk/"),
            new websiteInfo("Casino Share",             "www.luxurycasino.co.uk/en-gb/"),
            new websiteInfo("Casino Splendido",         "www.casinosplendido.com/"),
            new websiteInfo("Casino.com",               "www.casino.com/uk/"),
            new websiteInfo("Challenge Casino",         "www.luxurycasino.co.uk/en-gb/"),
            new websiteInfo("Conquer Casino",           "www.conquercasino.com/"),
            new websiteInfo("Cyber Club Casino",        "www.cyberclubcasino.com/"),
            new websiteInfo("Dash Casino",              "www.dashcasino.com/"),
            new websiteInfo("Dr Vegas Casino",          "www.drvegas.com/"),
            new websiteInfo("Dream Palace Casino",      "www.dreampalacecasino.com/"),
            new websiteInfo("EnergyCasino",             "energycasino.com/en/"),
            new websiteInfo("FruityCasa Casino",        "www.fruitycasa.com/"),
            new websiteInfo("Gala Casino",              "www.galacasino.com/"),
            new websiteInfo("GameVillage",              "www.gamevillage.com/"),
            new websiteInfo("Golden Lounge Casino",     "www.goldenlounge.com/"),
            new websiteInfo("Grosvenor Casino",         "www.grosvenorcasinos.com/"),
            new websiteInfo("Guts Casino",              "www.guts.com/gb/page/welcome"),
            new websiteInfo("Intercasino",              "www.intercasino.co.uk/"),
            new websiteInfo("Jackpot Luck Casino",      "www.jackpotluck.com/"),
            new websiteInfo("Jetbull Casino",           "www.jetbull.com/"),
            new websiteInfo("Karamba Casino",           "www.karamba.com/"),
            new websiteInfo("Ladbrokes Casino",         "casino.ladbrokes.com/en"),
            new websiteInfo("Magic Box Casino",         "www.magicboxcasino.com/"),
            new websiteInfo("Mansion Casino",           "play.mansioncasino.com/"),
            new websiteInfo("Maria Casino",             "www.mariacasino.co.uk/"),
            new websiteInfo("mFortune Casino",          "www.mfortune.co.uk/"),
            new websiteInfo("MobileWins Casino",        "www.mobilewins.co.uk/"),
            new websiteInfo("Monte Carlo Casino",       "www.casinomontecarlo.com/"),
            new websiteInfo("Moon Games Casino",        "www.moongames.com/"),
            new websiteInfo("Mr Green Casino",          "www.mrgreen.com/en"),
            new websiteInfo("Nedplay Casino",           "www.nedplay.com/"),
            new websiteInfo("Noxwin Casino",            "www.noxwin.com/#/"),
            new websiteInfo("Oddsring Casino",          "www.oddsring.com/home"),
            new websiteInfo("Paddy Power Casino",       "casino.paddypower.com/"),
            new websiteInfo("PokerStars Casino",        "www.pokerstarscasino.uk/"),
            new websiteInfo("Power Slots",              "www.powerslots.eu/"),
            new websiteInfo("Prospect Hall Casino",     "prospecthallcasino.com/games/index/"),
            new websiteInfo("Spinit Casino",            "www.spinit.com/en"),
            new websiteInfo("Redbet Casino",            "www.redbet.com/en/casino"),
            new websiteInfo("Red Queen Casino",         "www.redqueencasino.com/"),
            new websiteInfo("Rizk Casino",              "rizk.com/gb"),
            new websiteInfo("Roxy Palace Casino",       "www.roxypalace.com/"),
            new websiteInfo("Royal Swipe Casino",       "www.royalswipe.com/"),
            new websiteInfo("SCasino",                  "www.scasino.com/uk/"),
            new websiteInfo("Sportingbet Casino",       "casino.sportingbet.com/en/casino"),
            new websiteInfo("ShadowBet Casino",         "www.shadowbet.com/uk"),
            new websiteInfo("Slotty Vegas Casino",      "slottyvegas.com/en/welcome/"),
            new websiteInfo("Sporting Index Casino",    "casino.sportingindex.com/"),
            new websiteInfo("Trada Casino",             "www.tradacasino.com/"),
            new websiteInfo("Unibet Casino",            "www.unibet.co.uk/casino#filter:uk-unibet-picks-casino-slots-7-420439"),
            new websiteInfo("Vegas Paradise Casino",    "www.vegasparadise.com/"),
            new websiteInfo("VideoSlots Casino",        "www.videoslots.com/"),
            new websiteInfo("William Hill Casino",      "casino.williamhill.com/#!/"),
        };

        /// <summary>
        /// Get the list of Casinos
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfCasinos()
        {
            return listOfCasinos;
        }

        // currency exchanges

        private static readonly websiteInfo[] listOfCurrencyExchanges = 
        {
            new websiteInfo("#1 Currency",                      "www.no1currency.com/"),
            new websiteInfo("Ace-FX",                           "www.ace-fx.com/"),
            new websiteInfo("American Express",                 "www.americanexpress.com/uk/content/foreign-exchange/foreign-exchange-services.html"),
            new websiteInfo("Asda Travel Money",                "money.asda.com/travel-money/"),
            new websiteInfo("Barclays Bureau de change",        "www.barclays.co.uk/travel/foreign-currency-exchange/"),
            new websiteInfo("Barrhead Travel",                  "www.barrheadtravel.co.uk/foreign-exchange"),
            new websiteInfo("Best Exchange",                    "www.bestexchange.co.uk/"),
            new websiteInfo("Best Foreign Exchange",            "www.bestforeignexchange.com/"),
            new websiteInfo("BFC Exchange",                     "www.bfcexchange.co.uk/"),
            new websiteInfo("Central FX",                       "www.centralfx.co.uk/"),
            new websiteInfo("City Forex",                       "www.cityforex.co.uk/"),
            new websiteInfo("Change Group",                     "www.changegroup.co.uk/"),
            new websiteInfo("Compare Holiday Money",            "www.compareholidaymoney.com/"),
            new websiteInfo("Covent Garden FX",                 "www.coventgardenfx.com/"),
            new websiteInfo("Currencies for you",               "www.currencies4you.com/"),
            new websiteInfo("Currency converter",               "www.currencyconverter.co.uk/"),
            new websiteInfo("Currency matters",                 "www.currencymatters.co.uk/"),
            new websiteInfo("Currency solutions",               "www.currencysolutions.co.uk/"),
            new websiteInfo("Currency UK",                      "www.currencyuk.co.uk/"),
            new websiteInfo("Euro Change",                      "www.eurochange.co.uk/"),
            new websiteInfo("Danske Bank",                      "danskebank.co.uk/personal/help/currency-converter/currency-converter"),
            new websiteInfo("Debenhams",                        "finance.debenhams.com/travel-money/"),
            new websiteInfo("Elavon",                           "www.elavon.co.uk/dcc.html"),
            new websiteInfo("Exchange Rates",                   "www.exchangerates.org.uk/"),
            new websiteInfo("First Choice",                     "www.firstchoice.co.uk/holiday/info/foreign-exchange"),
            new websiteInfo("First Rate",                       "www.firstrate.co.uk/"),
            new websiteInfo("Fourex",                           "www.fourex.co.uk/"), 
            new websiteInfo("Global Exchange",                  "www.globalexchange.co.uk/"),
            new websiteInfo("GCEN",                             "gcen.co.uk/"),
            new websiteInfo("Money",                            "www.money.co.uk/travel-money.htm"),
            new websiteInfo("H & T Group",                      "www.handt.co.uk/travel-money"),
            new websiteInfo("Halifax Travel Money",             "www.halifax.co.uk/travel/travel-money/"),
            new websiteInfo("Hargreaves Lansdowne",             "www.hl.co.uk/investment-services/currency-service/latest-currency-report/currency-converter-exchange-rates"),
            new websiteInfo("HiFX",                             "www.hifx.co.uk"),
            new websiteInfo("HSBC Expat",                       "www.expat.hsbc.com/1/2/hsbc-expat/foreign-exchange-currency"),
            new websiteInfo("HSBC Travel Money",                "www.hsbc.co.uk/1/2/travel-money"),
            new websiteInfo("ICICI Bank",                       "www.icicibank.co.uk/personal/travel-money.page"),
            new websiteInfo("Internation Currency Exchange",    "www.iceplc.com/"),
            new websiteInfo("Kanoo Foreign Exchange",           "www.kanoocurrency.co.uk/"),
            new websiteInfo("KBR Foreign Exchange",             "www.kbrfx.com/"),
            new websiteInfo("M & S Currency Exchange",          "bank.marksandspencer.com/travel/travel-money/currency-exchange-rates/"),
            new websiteInfo("Money Corp ",                      "www.moneycorp.com/uk/"),
            new websiteInfo("Money Saving Expert",              "travelmoney.moneysavingexpert.com/"),
            new websiteInfo("Natwest International",            "www.natwestinternational.com/nw/personal-banking/travel-and-international/g48/travel-money/currency-converter.ashx"),
            new websiteInfo("Northwest Money Exchange",         "www.northwestmoneyexchange.com/"),
            new websiteInfo("Post Office Money",                "www.postoffice.co.uk/foreign-currency"),
            new websiteInfo("RBS",                              "www.rbs.co.uk/personal/travel/g1/money/exchange-rates.ashx"),
            new websiteInfo("Reuters",                          "uk.reuters.com/business/currencies"),
            new websiteInfo("Ruislip Currency",                 "www.ruislipcurrency.co.uk/"),
            new websiteInfo("Saga Travel Money",                "www.saga.co.uk/insurance/travel-money.aspx"),
            new websiteInfo("Sainsbury’s Bank Travel Money",    "www.sainsburysbank.co.uk/travel/ins_travelmoney_tmo_skip"),
            new websiteInfo("Santander Travel Money",           "www.santander.co.uk/uk/current-accounts/ordering-travel-money"),
            new websiteInfo("Senil Cash & Go",                  "www.senli.co.uk/"),
            new websiteInfo("Smart Currency Business",          "www.smartcurrencybusiness.com/"),
            new websiteInfo("Smart Currency Exchange",          "www.smartcurrencyexchange.com/"),
            new websiteInfo("Sterling",                         "www.sterlingfx.co.uk/"),
            new websiteInfo("Tesco Travel Money",               "www.tescobank.com/travel-money/"),
            new websiteInfo("The Currency Club",                "www.thecurrencyclub.co.uk/"),
            new websiteInfo("The Money Shop",                   "www.themoneyshop.com/travel-money/"),
            new websiteInfo("Thomas Cook",                      "www.thomascook.com/travel-money/foreign-currency/"),
            new websiteInfo("Thomas Money Exchange",            "www.thomasexchangeglobal.co.uk/"),
            new websiteInfo("TorFX",                            "www.torfx.com/"),
            new websiteInfo("Travelex",                         "www.travelex.co.uk/"),
            new websiteInfo("WeSwap",                           "www.weswap.com/en/"),
            new websiteInfo("World First",                      "www.worldfirst.com/uk/foreign-exchange/"),
            new websiteInfo("UAE Exchange",                     "www.uaeexchange.com/gbr/"),
            new websiteInfo("XE",                               "www.xe.com/"),
        };

        /// <summary>
        /// Get the list of Currency Exchanges
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfCurrencyExchanges()
        {
            return listOfCurrencyExchanges;
        }

        // wealth managers

        private static readonly websiteInfo[] listOfWealthManagers = 
        {
            new websiteInfo("Aberdeen Asset Management",                "www.aberdeen-asset.co.uk/"),
            new websiteInfo("Aberdeen Asset Management Trust Centre",   "www.invtrusts.co.uk/investmenttrusts/"),
            new websiteInfo("Allianz Global Investors",                 "uk.allianzgi.com/role-gate-page"),
            new websiteInfo("Artemis Investment Management LLP",        "www.artemisfunds.com/"),
            new websiteInfo("Baillie Gifford",                          "www.bailliegifford.com/"),
            new websiteInfo("Barclays Wealth",                          "www.barclays.co.uk/wealth-management/"),
            new websiteInfo("Blackrock",                                "www.blackrock.com"),
            new websiteInfo("Brewin Dolphin",                           "www.brewin.co.uk/"),
            new websiteInfo("Cantab Asset Management",                  "www.cantabam.com/"),
            new websiteInfo("Capital",                                  "www.capital.co.uk/"),
            new websiteInfo("Capital International",                    "www.capital-iom.com/"),
            new websiteInfo("CBRE Global Investors",                    "www.cbreglobalinvestors.com/Pages/default.aspx"),
            new websiteInfo("CCLA",                                     "www.ccla.co.uk/"),
            new websiteInfo("Charles Stanley",                          "www.charles-stanley.co.uk/"),
            new websiteInfo("Citi",                                     "www.citibank.co.uk/personal/wealth-management-products.do"),
            new websiteInfo("City Asset Management Plc",                "www.city-asset.co.uk/"),
            new websiteInfo("Clifton asset management",                 "www.clifton-asset.co.uk/"),
            new websiteInfo("Close Brothers Asset Management",          "www.closebrothersam.com/"),
            new websiteInfo("EFG",                                      "www.efgam.com/home/Landing-Asset-Management.html"),
            new websiteInfo("Equester Capital Management",              "www.neptunefunds.com"),
            new websiteInfo("Fidelity Worldwide Investment",            "www.fidelity.co.uk/home"),
            new websiteInfo("Franklin Templeton",                       "www.franklintempleton.co.uk/"),
            new websiteInfo("GAM",                                      "www.gam.com/"),
            new websiteInfo("Hargreaves Lansdowne",                     "www.hl.co.uk/"),
            new websiteInfo("Hawksmoor investment management",          "www.hawksmoorim.co.uk/"),
            new websiteInfo("Heartwood investment management",          "www.heartwoodgroup.co.uk/"),
            new websiteInfo("Henderson Global Investors",               "www.janushenderson.com/ukpi"),
            new websiteInfo("Hermes Investment Management",             "www.hermes-investment.com/ukw/"),
            new websiteInfo("Interactive Investor",                     "www.iii.co.uk/funds"),
            new websiteInfo("Investec Bank",                            "www.investec.com/en_gb.html"),
            new websiteInfo("Invesco Perpetual",                        "www.invescoperpetual.co.uk/uk"),
            new websiteInfo("Kleinwort Hambros",                        "www.kleinworthambros.com/en/"),
            new websiteInfo("Lion Trust ",                              "www.liontrust.co.uk/"),
            new websiteInfo("London and Capital",                       "www.londonandcapital.com/"),
            new websiteInfo("M&G Securities Ltd",                       "www.mandg.co.uk/"),
            new websiteInfo("Majedie",                                  "www.majedie.com/"),
            new websiteInfo("Mattioli Woods",                           "www.mattioliwoods.com/"),
            new websiteInfo("Mayfair Capital",                          "www.mayfaircapital.co.uk/"),
            new websiteInfo("Money Farm",                               "www.moneyfarm.com/uk/"),
            new websiteInfo("Montanaro",                                "www.montanaro.co.uk/"),
            new websiteInfo("Morning Star",                             "www.morningstar.co.uk/uk/"),
            new websiteInfo("MunnyPot",                                 "www.munnypot.com/"),
            new websiteInfo("Newton Investment Management",             "www.newtonim.com/"),
            new websiteInfo("Nova Financial",                           "www.novia-financial.co.uk/"),
            new websiteInfo("Nutmeg",                                   "www.nutmeg.com/"),
            new websiteInfo("Old Mutual Wealth",                        "www.oldmutualwealth.co.uk/"),
            new websiteInfo("Prospect Wealth Management",               "prospectwealth.co.uk/"),
            new websiteInfo("Psigma investment maangement",             "www.psigma.com/pages/psigma-investment-management-landing.aspx"),
            new websiteInfo("Quilter Cheviot",                          "www.quiltercheviot.com/uk/private-client/"),
            new websiteInfo("Rathbones",                                "www.rathbones.com/"),
            new websiteInfo("Sanlam Life and Pensions UK Limited",      "www.sanlam.co.uk/home.aspx"),
            new websiteInfo("Saranac Partners",                         "www.saranacpartners.com/"),
            new websiteInfo("Scalable Capital",                         "uk.scalable.capital/"),
            new websiteInfo("St.Jame’s Place",                          "www.sjp.co.uk/"),
            new websiteInfo("Standard Life Investments",                "www.standardlifeinvestments.com/"),
            new websiteInfo("State Street Global Advisors",             "www.ssga.com/home.html"),
            new websiteInfo("Schroders",                                "www.schroders.com"),
            new websiteInfo("SVM Asset Management",                     "www.svmonline.co.uk/"),
            new websiteInfo("Swanest",                                  "swanest.com/"),
            new websiteInfo("T Rowe Price",                             "www3.troweprice.com/usis/corporate/en/home.html"),
            new websiteInfo("TAM",                                      "www.tamassetmanagement.com/"),
            new websiteInfo("Threadneedle Asset Management",            "www.mythreadneedle.com/"),
            new websiteInfo("Tilney Group",                             "www.tilney.co.uk/"),
            new websiteInfo("Troy Asset Management",                    "www.taml.co.uk/"),
            new websiteInfo("UBS Global Asset Management",              "www.ubs.com/global/en/asset-management.html"),
            new websiteInfo("Unicorn Asset Management",                 "www.unicornam.com/"),
            new websiteInfo("Vanguard Asset Management",                "www.vanguardinvestor.co.uk/"),
            new websiteInfo("Wealth Horizon",                           "www.wealthhorizon.com/"),
        };

        /// <summary>
        /// Get the list of Wealth Managers
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfWealthManagers()
        {
            return listOfWealthManagers;
        }

        // building societies

        private static readonly websiteInfo[] listOfBuildingSocieties = 
        {
            new websiteInfo("Bath Investment & Building Society",       "www.bathbuildingsociety.co.uk/"),
            new websiteInfo("Beverly Building Society",                 "beverleybs.co.uk/"),
            new websiteInfo("Britannia Savings",                        "britannia.co.uk/"),
            new websiteInfo("Buckinghamshire Building Society",         "www.bucksbs.co.uk/"),
            new websiteInfo("Cambridge Building Society",               "www.cambridgebs.co.uk/"),
            new websiteInfo("Chorley & District Building Society",      "www.chorleybs.co.uk/"),
            new websiteInfo("Coventry Building Society",                "www.coventrybuildingsociety.co.uk/"),
            new websiteInfo("Cumberland Building Society",              "www.cumberland.co.uk/"),
            new websiteInfo("Darlington Building Society",              "www.darlington.co.uk/"),
            new websiteInfo("Dudley Building Society",                  "www.dudleybuildingsociety.co.uk/"),
            new websiteInfo("Earl Shilton Building Society",            "www.esbs.co.uk/"),
            new websiteInfo("Ecology Building Society",                 "www.ecology.co.uk/"),
            new websiteInfo("Furness Building Society",                 "www.furnessbs.co.uk/"),
            new websiteInfo("Hanley Economic Building Society",         "www.thehanley.co.uk/"),
            new websiteInfo("Harpenden Building Society",               "www.harpendenbs.co.uk/"),
            new websiteInfo("Hinckley & Rugby Building Society",        "www.hrbs.co.uk/"),
            new websiteInfo("Holmesdale Building Society",              "www.theholmesdale.co.uk/"),
            new websiteInfo("Ipswich Building Society",                 "www.ibs.co.uk/"),
            new websiteInfo("Leeds Building Society",                   "www.leedsbuildingsociety.co.uk/"),
            new websiteInfo("Leek United Building Society",             "www.leekunited.co.uk/"),
            new websiteInfo("Loughborough Buildiong Society",           "www.theloughborough.co.uk/"),
            new websiteInfo("Manchester Building Society",              "www.themanchester.co.uk/"),
            new websiteInfo("Mansfield Building Society",               "mansfieldbs.co.uk/"),
            new websiteInfo("Market Harborough Building Society",       "www.mhbs.co.uk/"),
            new websiteInfo("Marsden Building Society",                 "www.themarsden.co.uk/"),
            new websiteInfo("Melton Mowbray Building Society",          "www.themelton.co.uk/"),
            new websiteInfo("Monmouthshire Building Society",           "www.monbs.com/"),
            new websiteInfo("National Counties Building Society",       "www.ncbs.co.uk/"),
            new websiteInfo("Newbury Building Society",                 "www.newbury.co.uk/"),
            new websiteInfo("Newcastle Building Society",               "www.newcastle.co.uk/"),
            new websiteInfo("Norwich & Peterborough Building Society",  "www.nandp.co.uk/"),
            new websiteInfo("Nottingham Building Society",              "www.thenottingham.com/"),
            new websiteInfo("Penrith Building Society",                 "www.penrithbuildingsociety.co.uk/"),
            new websiteInfo("Principality Building Society",            "www.principality.co.uk/"),
            new websiteInfo("Progressive Building Society",             "theprogressive.com/"),
            new websiteInfo("Scottish Building Society",                "www.scottishbs.co.uk/"),
            new websiteInfo("Saffron Building Society",                 "www.saffronbs.co.uk/"),
            new websiteInfo("Skipton Building Society",                 "www.skipton.co.uk/"),
            new websiteInfo("Stafford Railway Building Society",        "srbs.co.uk/"),
            new websiteInfo("Swansea Building Society",                 "www.swansea-bs.co.uk/"),
            new websiteInfo("Teachers Building Society",                "www.teachersbs.co.uk/"),
            new websiteInfo("Tipton & Coseley Building Society",        "www.thetipton.co.uk/"),
            new websiteInfo("West Bromwich Building Society",           "www.westbrom.co.uk/"),
            new websiteInfo("Yorkshire Building Society",               "www.ybs.co.uk/index.html"),
        };

        /// <summary>
        /// Get the list of Building Societies
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfBuildingSocieties()
        {
            return listOfBuildingSocieties;
        }

        // banks

        private static readonly websiteInfo[] listOfBanks = 
        {
            new websiteInfo("Abbey National Treasury Services Plc",                 "www.santander.co.uk/uk/about-santander-uk/investor-relations/abbey-national-treasury-services-plc"),
            new websiteInfo("ABC International Bank Plc",                           "www.bank-abc.com/world/ABCIB/en/Pages/default.aspx"),
            new websiteInfo("Access Bank UK Limited",                               "www.theaccessbankukltd.co.uk/"),
            new websiteInfo("Adam & Company Plc",                                   "www.adambank.com/"),
            new websiteInfo("ADIB(UK) Ltd",                                         "www.adib.co.uk/en/Pages/default.aspx"),
            new websiteInfo("Ahli United Bank(UK) PLC",                             "www.ahliunited.com/"),
            new websiteInfo("AIB Group(UK) Plc",                                    "group.aib.ie/"),
            new websiteInfo("Airdrie Savings Bank",                                 "airdriesavingsbank.com/"),
            new websiteInfo("Al Rayan Bank PLC",                                    "www.alrayanbank.co.uk/"),
            new websiteInfo("Aldermore Bank Plc",                                   "www.aldermore.co.uk/"),
            new websiteInfo("Alliance Trust Savings Limited",                       "www.alliancetrustsavings.co.uk/"),
            new websiteInfo("Alpha Bank London Limited",                            "www.alpha-bank.uk/"),
            new websiteInfo("ANZ Bank (Europe) Limited",                            "www.anz.com/unitedkingdom/en/personal/"),
            new websiteInfo("Arbuthnot Latham & Co Limited",                        "www.arbuthnotlatham.co.uk/"),
            new websiteInfo("Atom Bank PLC",                                        "www.atombank.co.uk/"),
            new websiteInfo("Axis Bank UK Limited",                                 "www.onlineaxisbankuk.co.uk"),

            new websiteInfo("Bank and Clients PLC",                                 "www.bankandclients.com/"),
            new websiteInfo("Bank Leumi(UK) plc",                                   "www.bankleumi.co.uk/"),
            new websiteInfo("Bank Mandiri(Europe) Limited",                         "www.bkmandiri.co.uk/"),
            new websiteInfo("Bank of America Merrill Lynch International Limited",  "www.bofaml.com/content/boaml/en_us/home.html"),
            new websiteInfo("Bank of Baroda",                                       "www.bankofbaroda.com/"),
            new websiteInfo("Bank of Beirut(UK) Ltd",                               "www.bankofbeirut.co.uk"),
            new websiteInfo("Bank of Ceylon(UK) Ltd",                               "www.bankofceylon.co.uk/"),
            new websiteInfo("Bank of China(UK) Ltd",                                "www.bankofchina.com/uk/"),
            new websiteInfo("Bank of Communications(UK)",                           "www.uk.bankcomm.com/BankCommSite/shtml/ygzh/en/8848/list.shtml?channelId=8848"),
            new websiteInfo("Bank of Cyprus UK Limited",                            "www.bankofcyprus.co.uk/"),
            new websiteInfo("Bank of India",                                        "www.bankofindia.co.in/english/home.aspx"),
            new websiteInfo("Bank of Ireland(UK) Plc",                              "bankofirelanduk.com/"),
            new websiteInfo("Bank of London and The Middle East plc",               "www.blme.com/"),
            new websiteInfo("Bank of New York Mellon (International) Limited",      "www.bnymellon.com/uk/en/index.jsp"),
            new websiteInfo("Bank of Scotland plc",                                 "www.bankofscotland.co.uk/"),
            new websiteInfo("Bank of the Philippine Islands (Europe)",              "www.bpiexpressonline.com/p/0/165/bpi-europe"),
            new websiteInfo("Bank Saderat Plc",                                     "www.saderat-plc.com/"),
            new websiteInfo("Bank Sepah International Plc",                         "www.banksepah.co.uk/"),
            new websiteInfo("Barclays Bank Plc",                                    "www.barclays.co.uk/"),
            new websiteInfo("BFC Exchange Ltd",                                     "www.bfcexchange.co.uk/"),
            new websiteInfo("BIRA Bank Ltd",                                        "bira.co.uk/services/bank/"),
            new websiteInfo("BMCE Bank International plc",                          "www.bmce-intl.co.uk/disclaimer.html"),
            new websiteInfo("British Arab Commercial Bank Plc",                     "www.bacb.co.uk/"),
            new websiteInfo("Brown Shipley & Co Limited",                           "www.brownshipley.com/"),

            new websiteInfo("C Hoare & Co",                                         "www.hoaresbank.co.uk/"),
            new websiteInfo("CAF Bank Ltd",                                         "secure.cafbank.org/"),
            new websiteInfo("Cambridge & Counties Bank Limited",                    "ccbank.co.uk/"),
            new websiteInfo("Cater Allen Limited",                                  "www.caterallen.co.uk/"),
            new websiteInfo("Charity Bank Limited",                                 "charitybank.org/"),
            new websiteInfo("Charter Court Financial Services Limited",             "www.chartercourtfs.co.uk/"),
            new websiteInfo("China Construction Bank(London) Limited",              "www.uk.ccb.com/london/en/index.html"),
            new websiteInfo("CIBC World Markets Plc",                               "www.cibcwm.com/cibc-eportal-web/portal/wm?pageId=home&language=en_CA"),
            new websiteInfo("ClearBank Ltd",                                        "www.clear.bank/"),
            new websiteInfo("Close Brothers Limited",                               "www.closebrothers.com/"),
            new websiteInfo("Clydesdale Bank Plc CYBG plc",                         "www.cybg.com/"),
            new websiteInfo("Co-operative Bank Plc",                                "www.co-operativebank.co.uk/"),
            new websiteInfo("Coutts & Company",                                     "www.coutts.com/"),
            new websiteInfo("Credit Suisse(UK) Limited",                            "www.credit-suisse.com/uk/en.html"),
            new websiteInfo("Credit Suisse International Credit Suisse",            "www.credit-suisse.com/uk/en/investment-banking/financial-regulatory/international.html"),
            new websiteInfo("Crown Agents Bank Limited",                            "www.crownagentsbank.com/"),

            new websiteInfo("DB UK Bank Limited",                                   "www.db.com/unitedkingdom/"),
            new websiteInfo("Diamond Bank (UK) Plc",                                "diamondbankukplc.com/"),
            new websiteInfo("Duncan Lawrie Limited",                                "www.camellia.plc.uk/duncan-lawrie"),

            new websiteInfo("EFG Private Bank Limited",                             "www.efgl.com/"),
            new websiteInfo("Europe Arab Bank plc",                                 "www.eabplc.com/"),

            new websiteInfo("First Direct",                                         "www1.firstdirect.com/1/2/"),
            new websiteInfo("FBN Bank(UK) Ltd",                                     "www.fbnbank.co.uk/"),
            new websiteInfo("FCE Bank Plc",                                         "www.fcebank.com/"),
            new websiteInfo("FCMB Bank(UK) Limited",                                "www.fcmbuk.com/"),

            new websiteInfo("Gatehouse Bank Plc",                                   "www.gatehousebank.com/"),
            new websiteInfo("GE Capital Bank Limited GE Capital",                   "www.gecapital.co.uk/en/"),
            new websiteInfo("Ghana International Bank Plc",                         "www.ghanabank.co.uk/"),
            new websiteInfo("Goldman Sachs International Bank",                     "www.goldmansachs.com/"),
            new websiteInfo("Guaranty Trust Bank (UK) Limited",                     "www.gtbankuk.com/"),
            new websiteInfo("Gulf International Bank (UK) Limited",                 "www.gib.com/"),

            new websiteInfo("Habib Bank Zurich Plc",                                "www.habibbank.com/uk/home/ukHome.html"),
            new websiteInfo("Habibsons Bank Limited",                               "habibbankuk.com/"),
            new websiteInfo("Halifax",                                              "www.halifax.co.uk/"),
            new websiteInfo("Hampden & Co Plc",                                     "www.hampdenandco.com/"),
            new websiteInfo("Hampshire Trust Bank Plc",                             "www.htb.co.uk/"),
            new websiteInfo("Harrods Bank Ltd",                                     "www.harrodsbank.co.uk/"),
            new websiteInfo("Havin Bank Ltd",                                       "www.havanaintbank.co.uk/"),
            new websiteInfo("HSBC Bank Plc",                                        "www.hsbc.co.uk/1/2/"),
            new websiteInfo("HSBC Private Bank(UK) Limited",                        "www.hsbcprivatebank.com/en"),
            
            new websiteInfo("ICBC(London) plc",                                     "www.icbclondon.com/icbc/%E6%B5%B7%E5%A4%96%E5%88%86%E8%A1%8C/%E5%B7%A5%E9%93%B6%E4%BC%A6%E6%95%A6%E7%BD%91%E7%AB%99/en/"),
            new websiteInfo("ICBC Standard Bank Plc",                               "www.icbcstandardbank.com/CorporateSite"),
            new websiteInfo("ICICI Bank UK Plc",                                    "www.icicibank.co.uk/"),
            new websiteInfo("Investec Bank PLC",                                    "www.investec.com/en_gb.html"),
            new websiteInfo("Itau BBA International PLC",                           "www.itau.com.br/itaubba-en"),

            new websiteInfo("J.P.Morgan Europe Limited",                            "www.jpmorgan.com/country/GB/en/jpmorgan"),
            new websiteInfo("J.P.Morgan Securities plc",                            "www.jpmorgansecurities.com/"),
            new websiteInfo("Jordan International Bank Plc",                        "www.jordanbank.co.uk/"),
            new websiteInfo("Julian Hodge Bank Limited",                            "www.hodgebank.co.uk/"),

            new websiteInfo("Kexim Bank(UK) Ltd",                                   "srssprojects.in/aboutus.html"),
            new websiteInfo("Kingdom Bank Ltd",                                     "www.kingdom.bank/"),
            new websiteInfo("Kleinwort Benson Bank Ltd",                            "www.kleinworthambros.com/en/"),
            new websiteInfo("Kookmin Bank International Limited",                   "www.kbfg.com/Eng/"),

            new websiteInfo("Lloyds Bank Plc",                                      "www.lloydsbank.com/"),
            new websiteInfo("Lloyds Bank Private Banking Limited",                  "www.lloydsbank.com/private-banking/home.asp"),
            new websiteInfo("Lloyds Banking Group",                                 "www.lloydsbankinggroup.com/"),

            new websiteInfo("Macquarie Bank International Ltd",                     "www.macquarie.com/uk/corporate"),
            new websiteInfo("Marks & Spencer Financial Services Plc",               "bank.marksandspencer.com/"),
            new websiteInfo("Masthaven Bank Limited",                               "www.masthaven.co.uk/"),
            new websiteInfo("Melli Bank plc",                                       "www.mellibank.com/"),
            new websiteInfo("Methodist Chapel Aid Limited",                         "www.mcafundingforchurches.co.uk/"),
            new websiteInfo("Metro Bank PLC",                                       "www.metrobankonline.co.uk/"),
            new websiteInfo("Mizuho International Plc",                             "www.mizuho-emea.com/"),
            new websiteInfo("Monzo Bank Ltd",                                       "monzo.com/"),
            new websiteInfo("Morgan Stanley Bank International Limited",            "www.morganstanley.com/"),

            new websiteInfo("National Bank of Egypt(UK) Limited",                   "www.nbeuk.com/"),
            new websiteInfo("National Bank of Kuwait(International) Plc",           "nbk.com/"),
            new websiteInfo("National Westminster Bank Plc",                        "personal.natwest.com/"),

            new websiteInfo("Natwest International",                                "www.natwestinternational.com/nw/personal-banking.ashx"),

            new websiteInfo("Nationwide Building Society",                          "www.nationwide.co.uk/"),
            new websiteInfo("Nomura Bank International Plc",                        "www.nomura.com/"),
            new websiteInfo("Northern Bank Limited",                                "danskebank.co.uk/personal"),
            new websiteInfo("Northern Trust Global Services Ltd",                   "www.northerntrust.com/"),

            new websiteInfo("OakNorth Bank Limited",                                "www.oaknorth.com/"),
            new websiteInfo("OneSavings Bank Plc",                                  "www.osb.co.uk/"),

            new websiteInfo("Paragon Bank Plc",                                     "www.paragonbank.co.uk/"),
            new websiteInfo("PCF Group Holdings Ltd",                               "pcf.bank/"),
            new websiteInfo("Persia International Bank Plc",                        "persiabank.co.uk/"),
            new websiteInfo("Philippine National Bank(Europe) Plc",                 "www.pnb.com.ph/europe/"),
            new websiteInfo("Punjab National Bank(International) Limited",          "www.pnbint.com/"),

            new websiteInfo("QIB(UK) Plc",                                          "www.qib-uk.com/en/index.aspx"),

            new websiteInfo("R.Raphael & Sons Plc",                                 "www.raphaelsbank.com/"),
            new websiteInfo("Rathbone Investment Management Limited",               "www.rathbones.com/"),
            new websiteInfo("RBC Europe Limited",                                   "www.rbc.com/contactus/rbc_europe.html"),
            new websiteInfo("Reliance Bank Ltd",                                    "www.reliancebankltd.com/"),
            new websiteInfo("Revolut",                                              "www.revolut.com/?lang=en"),
            new websiteInfo("Royal Bank of Scotland Plc",                           "personal.rbs.co.uk/personal.html"),

            new websiteInfo("Sainsbury’s Bank Plc",                                 "www.sainsburysbank.co.uk/"),
            new websiteInfo("Santander UK Plc",                                     "www.santander.co.uk/uk/index"),
            new websiteInfo("Schroder & Co Ltd",                                    "www.schroders.com/"),
            new websiteInfo("Scotiabank Europe Plc",                                "www.scotiabank.com/global/en/0,,6182,00.html"),
            new websiteInfo("Scottish Widows Bank Plc",                             "www.scottishwidows.co.uk/bank/"),
            new websiteInfo("Secure Trust Bank Plc",                                "www.securetrustbank.com/"),
            new websiteInfo("SG Hambros Bank Limited",                              "www.societegenerale.co.uk/en/worldwide-details/office/head-office/"),
            new websiteInfo("Shawbrook Bank Limited",                               "www.shawbrook.co.uk/"),
            new websiteInfo("Smith & Williamson Investment Services Limited",       "smithandwilliamson.com/"),
            new websiteInfo("Sonali Bank(UK) Limited",                              "www.sonali-bank.com/"),
            new websiteInfo("Standard Chartered Bank",                              "www.sc.com/en/"),
            new websiteInfo("Starling Bank Limited",                                "www.starlingbank.com/"),
            new websiteInfo("State Bank of India",                                  "www.onlinesbi.com/"),
            new websiteInfo("Sumitomo Mitsui Banking Corporation Europe Limited",   "www.smbcgroup.com/emea/info/smbce"),

            new websiteInfo("Tandem Bank Limited",                                  "www.tandem.co.uk/"),
            new websiteInfo("TD Bank Europe Limited",                               "www.td.com/about-tdbfg/our-business/index.jsp"),
            new websiteInfo("Tesco Personal Finance Plc",                           "www.tescobank.com/"),
            new websiteInfo("TSB Bank plc",                                         "www.tsb.co.uk/personal/"),
            new websiteInfo("Turkish Bank (UK) Ltd",                                "www.turkishbank.co.uk/"),

            new websiteInfo("UBS Limited",                                          "www.ubs.com/uk/en.html"),
            new websiteInfo("Ulster Bank Ltd",                                      "digital.ulsterbank.co.uk/"),
            new websiteInfo("Union Bank of India(UK) Limited",                      "www.unionbankofindiauk.co.uk/"),
            new websiteInfo("Union Bank UK Plc",                                    "www.unionbankuk.co.uk/netbanking/"),
            new websiteInfo("United National Bank Limited",                         "www.ubluk.com/"),
            new websiteInfo("United Trust Bank Limited",                            "www.utbank.co.uk/"),
            new websiteInfo("Unity Trust Bank Plc",                                 "www.unity.co.uk/"),

            new websiteInfo("Vanquis Bank Limited",                                 "www.vanquis.co.uk/"),
            new websiteInfo("Virgin Money plc",                                     "uk.virginmoney.com/virgin/"),
            new websiteInfo("VTB Capital plc",                                      "www.vtbcapital.com/"),

            new websiteInfo("Weatherbys Bank Limited",                              "www.weatherbys.bank/"),
            new websiteInfo("Wesleyan Bank Limited",                                "www.wesleyan.co.uk/wesleyan-bank/"),
            new websiteInfo("Westpac Europe Ltd",                                   "www.westpac.com.au/about-westpac/global-locations/westpac-uk/"),
            new websiteInfo("Wyelands Bank Plc",                                    "www.wyelandsbank.co.uk/"),

            new websiteInfo("Zenith Bank(UK) Limited",                              "www.zenith-bank.co.uk/"),
        };

        /// <summary>
        /// Get the list of Banks
        /// </summary>
        /// <returns>An array of websites to test.</returns>
        public static websiteInfo[] getListOfBanks()
        {
            return listOfBanks;
        }
    }
}
