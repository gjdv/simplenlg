using System;
using System.Collections.Generic;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class InflVarsAndAgreements
    {
        public InflVarsAndAgreements(LexRecord lexRecord)
        {
            lexRecord_ = lexRecord;
            string @base = lexRecord.GetBase();
            string cit = lexRecord.GetBase();
            string eui = lexRecord.GetEui();
            string cat = lexRecord.GetCategory();

            GetInflVarsAndAgreements(@base, eui, cat, cit);

            List<string> sv = lexRecord.GetSpellingVars();
            for (int i = 0; i < sv.Count; i++)
            {
                string svBase = (string) sv[i];
                GetInflVarsAndAgreements(svBase, eui, cat, cit);
            }
        }


        public static string GetXml(LexRecord lexRecord)
        {
            bool convertFlag = true;
            InflVarsAndAgreements inflVars = new InflVarsAndAgreements(lexRecord);
            string @out = "";
            List<InflVar> inflValues = inflVars.GetInflValues();
            for (int i = 0; i < inflValues.Count; i++)
            {
                InflVar inflectionVar = (InflVar) inflValues[i];
                string typeAttribute = "";
                if (inflectionVar.GetType() != null)
                {
                    typeAttribute = " type=\"" + inflectionVar.GetType() + "\"";
                }


                string startTag = "<inflVars cat=\"" + inflectionVar.GetCat() + "\" cit=\"" +
                                  Convert.ToNumericEntity(inflectionVar.GetCit()) + "\" eui=\"" +
                                  inflectionVar.GetEui() + "\" infl=\"" + inflectionVar.GetInflection() + "\"" +
                                  typeAttribute + " unInfl=\"" + Convert.ToNumericEntity(inflectionVar.GetUnInfl()) +
                                  "\">";

                @out = XmlLib.AddToXml(@out, startTag, "</inflVars>", inflectionVar.GetVar(), 2, convertFlag);
            }

            return @out;
        }


        public virtual List<InflVar> GetInflValues()
        {
            return inflValues_;
        }


        public virtual List<Agreement> GetAgreementValues()
        {
            return agrValues_;
        }


        private void GetInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            if (cat == "verb")
            {
                GetVerbInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
            else if (cat == "noun")
            {
                GetNounInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
            else if (cat == "adj")
            {
                GetAdjInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
            else if (cat == "adv")
            {
                GetAdvInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
            else if (cat == "aux")
            {
                GetAuxInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
            else if (cat == "modal")
            {
                GetModalInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
            else if (cat == "pron")
            {
                GetPronInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
            else if (cat == "det")
            {
                GetDetInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
            else if (cat == "prep")
            {
                GetPrepInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
            else if (cat == "compl")
            {
                GetComplInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
            else if (cat == "conj")
            {
                GetConjInflVarsAndAgreements(unInfl, eui, cat, cit);
            }
        }


        private void GetVerbInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));


            inflValues_.Add(new InflVar(eui, unInfl, "pres1p23p", unInfl, cat, cit));

            agrValues_.Add(new Agreement(eui, unInfl, cat, "pres(fst_sing,fst_plur,thr_plur,second)", unInfl,
                cit));

            InflVar infinitive = new InflVar(eui, unInfl, "infinitive", unInfl, cat, cit);

            Agreement agrInf = new Agreement(eui, unInfl, cat, "infinitive", unInfl, cit);


            List<string> variants = lexRecord_.GetCatEntry().GetVerbEntry().GetVariants();
            string inflVar = "";
            for (int i = 0; i < variants.Count; i++)
            {
                string variant = (string) variants[i];
                if (variant.StartsWith("regd", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(infinitive))
                    {
                        inflValues_.Add(infinitive);
                    }

                    if (!agrValues_.Contains(agrInf))
                    {
                        agrValues_.Add(agrInf);
                    }

                    string type = "regd";
                    char lastChar = GetLastChar(unInfl);
                    char last2Char = GetLast2Char(unInfl);
                    string lastCharStr = (new char?(lastChar)).ToString();
                    string last2CharStr = (new char?(last2Char)).ToString();

                    if ((!consonants_.Contains(lastCharStr)) || (!vowels_.Contains(last2CharStr)))
                    {
                        Console.Error.WriteLine("** Err@regd violation: " + eui + "|" + unInfl + "|" + cat);
                    }

                    InflVar pres3s = null;

                    if ((unInfl.EndsWith("s", StringComparison.Ordinal)) ||
                        (unInfl.EndsWith("z", StringComparison.Ordinal)) ||
                        (unInfl.EndsWith("x", StringComparison.Ordinal)) ||
                        (unInfl.EndsWith("ch", StringComparison.Ordinal)) ||
                        (unInfl.EndsWith("sh", StringComparison.Ordinal)))
                    {
                        inflVar = unInfl + "es";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));
                    }
                    else if (unInfl.EndsWith("ie", StringComparison.Ordinal))
                    {
                        inflVar = unInfl + "s";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));
                    }
                    else if ((unInfl.EndsWith("ee", StringComparison.Ordinal)) ||
                             (unInfl.EndsWith("oe", StringComparison.Ordinal)) ||
                             (unInfl.EndsWith("ye", StringComparison.Ordinal)))
                    {
                        inflVar = unInfl + "s";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));
                    }
                    else if ((unInfl.EndsWith("y", StringComparison.Ordinal)) && (consonants_.Contains(last2CharStr)))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ies";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));
                    }
                    else if ((unInfl.EndsWith("e", StringComparison.Ordinal)) && (!eioySets_.Contains(last2CharStr)))
                    {
                        inflVar = unInfl + "s";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));
                    }
                    else
                    {
                        inflVar = unInfl + "s";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));
                    }

                    pres3s.SetUnique(IsUnique(inflValues_, pres3s));
                    inflValues_.Add(pres3s);

                    inflVar = unInfl + lastChar + "ed";
                    inflValues_.Add(new InflVar(eui, unInfl, type, "past", inflVar, cat, cit));

                    agrValues_.Add(new Agreement(eui, inflVar, cat, "past", unInfl, cit));


                    inflVar = unInfl + lastChar + "ed";
                    inflValues_.Add(new InflVar(eui, unInfl, type, "pastPart", inflVar, cat, cit));

                    agrValues_.Add(new Agreement(eui, inflVar, cat, "past_part", unInfl, cit));


                    inflVar = unInfl + lastChar + "ing";
                    inflValues_.Add(new InflVar(eui, unInfl, type, "presPart", inflVar, cat, cit));

                    agrValues_.Add(new Agreement(eui, inflVar, cat, "pres_part", unInfl, cit));
                }
                else if (variant.StartsWith("reg", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(infinitive))
                    {
                        inflValues_.Add(infinitive);
                    }

                    if (!agrValues_.Contains(agrInf))
                    {
                        agrValues_.Add(agrInf);
                    }

                    string type = "reg";
                    char lastChar = GetLastChar(unInfl);
                    char last2Char = GetLast2Char(unInfl);
                    string last2CharStr = (new char?(last2Char)).ToString();
                    InflVar pres3s = null;
                    InflVar past = null;
                    InflVar pastPart = null;
                    InflVar presPart = null;
                    if ((unInfl.EndsWith("s", StringComparison.Ordinal)) ||
                        (unInfl.EndsWith("z", StringComparison.Ordinal)) ||
                        (unInfl.EndsWith("x", StringComparison.Ordinal)) ||
                        (unInfl.EndsWith("ch", StringComparison.Ordinal)) ||
                        (unInfl.EndsWith("sh", StringComparison.Ordinal)))
                    {
                        inflVar = unInfl + "es";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));


                        inflVar = unInfl + "ed";
                        past = new InflVar(eui, unInfl, type, "past", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past", unInfl, cit));


                        inflVar = unInfl + "ed";
                        pastPart = new InflVar(eui, unInfl, type, "pastPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past_part", unInfl, cit));


                        inflVar = unInfl + "ing";
                        presPart = new InflVar(eui, unInfl, type, "presPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres_part", unInfl, cit));
                    }
                    else if (unInfl.EndsWith("ie", StringComparison.Ordinal))
                    {
                        inflVar = unInfl + "s";

                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));


                        inflVar = unInfl + "d";
                        past = new InflVar(eui, unInfl, type, "past", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past", unInfl, cit));


                        inflVar = unInfl + "d";
                        pastPart = new InflVar(eui, unInfl, type, "pastPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past_part", unInfl, cit));


                        inflVar = unInfl.Substring(0, unInfl.Length - 2) + "ying";
                        presPart = new InflVar(eui, unInfl, type, "presPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres_part", unInfl, cit));
                    }
                    else if ((unInfl.EndsWith("ee", StringComparison.Ordinal)) ||
                             (unInfl.EndsWith("oe", StringComparison.Ordinal)) ||
                             (unInfl.EndsWith("ye", StringComparison.Ordinal)))
                    {
                        inflVar = unInfl + "s";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));


                        inflVar = unInfl + "d";
                        past = new InflVar(eui, unInfl, type, "past", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past", unInfl, cit));


                        inflVar = unInfl + "d";
                        pastPart = new InflVar(eui, unInfl, type, "pastPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past_part", unInfl, cit));


                        inflVar = unInfl + "ing";
                        presPart = new InflVar(eui, unInfl, type, "presPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres_part", unInfl, cit));
                    }
                    else if ((unInfl.EndsWith("y", StringComparison.Ordinal)) && (consonants_.Contains(last2CharStr)))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ies";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));


                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ied";
                        past = new InflVar(eui, unInfl, type, "past", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past", unInfl, cit));


                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ied";
                        pastPart = new InflVar(eui, unInfl, type, "pastPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past_part", unInfl, cit));


                        inflVar = unInfl + "ing";
                        presPart = new InflVar(eui, unInfl, type, "presPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres_part", unInfl, cit));
                    }
                    else if ((unInfl.EndsWith("e", StringComparison.Ordinal)) && (!eioySets_.Contains(last2CharStr)))
                    {
                        inflVar = unInfl + "s";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));


                        inflVar = unInfl + "d";
                        past = new InflVar(eui, unInfl, type, "past", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past", unInfl, cit));


                        inflVar = unInfl + "d";
                        pastPart = new InflVar(eui, unInfl, type, "pastPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past_part", unInfl, cit));


                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ing";
                        presPart = new InflVar(eui, unInfl, type, "presPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres_part", unInfl, cit));
                    }
                    else
                    {
                        inflVar = unInfl + "s";
                        pres3s = new InflVar(eui, unInfl, type, "pres3s", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres(thr_sing)", unInfl, cit));


                        inflVar = unInfl + "ed";
                        past = new InflVar(eui, unInfl, type, "past", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past", unInfl, cit));


                        inflVar = unInfl + "ed";
                        pastPart = new InflVar(eui, unInfl, type, "pastPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "past_part", unInfl, cit));


                        inflVar = unInfl + "ing";
                        presPart = new InflVar(eui, unInfl, type, "presPart", inflVar, cat, cit);

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "pres_part", unInfl, cit));
                    }

                    pres3s.SetUnique(IsUnique(inflValues_, pres3s));
                    past.SetUnique(IsUnique(inflValues_, past));
                    pastPart.SetUnique(IsUnique(inflValues_, pastPart));
                    presPart.SetUnique(IsUnique(inflValues_, presPart));
                    inflValues_.Add(pres3s);
                    inflValues_.Add(past);
                    inflValues_.Add(pastPart);
                    inflValues_.Add(presPart);
                }
                else if (variant.StartsWith("irreg", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(infinitive))
                    {
                        inflValues_.Add(infinitive);
                    }

                    if (!agrValues_.Contains(agrInf))
                    {
                        agrValues_.Add(agrInf);
                    }

                    int index1 = variant.IndexOf("|", StringComparison.Ordinal);
                    int index2 = variant.IndexOf("|", index1 + 1, StringComparison.Ordinal);
                    int index3 = variant.IndexOf("|", index2 + 1, StringComparison.Ordinal);
                    int index4 = variant.IndexOf("|", index3 + 1, StringComparison.Ordinal);
                    int index5 = variant.IndexOf("|", index4 + 1, StringComparison.Ordinal);
                    int index6 = variant.IndexOf("|", index5 + 1, StringComparison.Ordinal);
                    string type = variant.Substring(0, index1);
                    string baseStr = variant.Substring(index1 + 1, index2 - (index1 + 1));
                    string pres3sStr = variant.Substring(index2 + 1, index3 - (index2 + 1));
                    string pastStr = variant.Substring(index3 + 1, index4 - (index3 + 1));
                    string pastPartStr = variant.Substring(index4 + 1, index5 - (index4 + 1));
                    string presPartStr = variant.Substring(index5 + 1, index6 - (index5 + 1));
                    if (unInfl.Equals(baseStr) == true)
                    {
                        if (pres3sStr.Length > 0)
                        {
                            InflVar pres3s = new InflVar(eui, unInfl, type, "pres3s", pres3sStr, cat, cit);

                            pres3s.SetUnique(IsUnique(inflValues_, pres3s));
                            inflValues_.Add(pres3s);
                            agrValues_.Add(new Agreement(eui, pres3sStr, cat, "pres(thr_sing)", unInfl, cit));
                        }


                        if (pastStr.Length > 0)
                        {
                            InflVar past = new InflVar(eui, unInfl, type, "past", pastStr, cat, cit);

                            past.SetUnique(IsUnique(inflValues_, past));
                            inflValues_.Add(past);
                            agrValues_.Add(new Agreement(eui, pastStr, cat, "past", unInfl, cit));
                        }


                        if (pastPartStr.Length > 0)
                        {
                            InflVar pastPart = new InflVar(eui, unInfl, type, "pastPart", pastPartStr, cat, cit);

                            pastPart.SetUnique(IsUnique(inflValues_, pastPart));
                            inflValues_.Add(pastPart);
                            agrValues_.Add(new Agreement(eui, pastPartStr, cat, "past_part", unInfl, cit));
                        }


                        if (presPartStr.Length > 0)
                        {
                            InflVar presPart = new InflVar(eui, unInfl, type, "presPart", presPartStr, cat, cit);

                            presPart.SetUnique(IsUnique(inflValues_, presPart));
                            inflValues_.Add(presPart);
                            agrValues_.Add(new Agreement(eui, presPartStr, cat, "pres_part", unInfl, cit));
                        }
                    }
                }
                else
                {
                    Console.WriteLine("-- Warning: InflVarsAndAgreements.GetVerbInflVarsAndAgreements()");
                }
            }
        }

        private void GetNounInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));

            InflVar singular = new InflVar(eui, unInfl, "singular", unInfl, cat, cit);


            Agreement singAgr = new Agreement(eui, unInfl, cat, "count(thr_sing)", unInfl, cit);


            List<string> variants = lexRecord_.GetCatEntry().GetNounEntry().GetVariants();
            string inflVar = "";
            for (int i = 0; i < variants.Count; i++)
            {
                string variant = (string) variants[i];
                if (variant.StartsWith("reg", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(singular))
                    {
                        inflValues_.Add(singular);
                    }

                    if (!agrValues_.Contains(singAgr))
                    {
                        agrValues_.Add(singAgr);
                    }

                    char lastChar = GetLastChar(unInfl);
                    char last2Char = GetLast2Char(unInfl);
                    string last2CharStr = (new char?(last2Char)).ToString();
                    InflVar plural = null;
                    if ((lastChar == 'y') && (consonants_.Contains(last2CharStr)))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ies";
                        plural = new InflVar(eui, unInfl, "reg", "plural", inflVar, cat, cit);
                    }
                    else if ((unInfl.EndsWith("s", StringComparison.Ordinal)) ||
                             (unInfl.EndsWith("z", StringComparison.Ordinal)) ||
                             (unInfl.EndsWith("x", StringComparison.Ordinal)) ||
                             (unInfl.EndsWith("ch", StringComparison.Ordinal)) ||
                             (unInfl.EndsWith("sh", StringComparison.Ordinal)))
                    {
                        inflVar = unInfl + "es";
                        plural = new InflVar(eui, unInfl, "reg", "plural", inflVar, cat, cit);
                    }
                    else
                    {
                        inflVar = unInfl + "s";
                        plural = new InflVar(eui, unInfl, "reg", "plural", inflVar, cat, cit);
                    }

                    plural.SetUnique(IsUnique(inflValues_, plural));
                    inflValues_.Add(plural);
                    agrValues_.Add(new Agreement(eui, inflVar, cat, "count(thr_plur)", unInfl, cit));
                }
                else if (variant.StartsWith("glreg", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(singular))
                    {
                        inflValues_.Add(singular);
                    }

                    if (!agrValues_.Contains(singAgr))
                    {
                        agrValues_.Add(singAgr);
                    }

                    inflVar = unInfl;
                    if (unInfl.EndsWith("us", StringComparison.Ordinal))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 2) + "i";
                        inflValues_.Add(new InflVar(eui, unInfl, "glreg", "plural", inflVar, cat, cit));
                    }
                    else if (unInfl.EndsWith("ma", StringComparison.Ordinal))
                    {
                        inflVar = unInfl + "ta";
                        inflValues_.Add(new InflVar(eui, unInfl, "glreg", "plural", inflVar, cat, cit));
                    }
                    else if (unInfl.EndsWith("a", StringComparison.Ordinal))
                    {
                        inflVar = unInfl + "e";
                        inflValues_.Add(new InflVar(eui, unInfl, "glreg", "plural", inflVar, cat, cit));
                    }
                    else if (unInfl.EndsWith("um", StringComparison.Ordinal))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 2) + "a";
                        inflValues_.Add(new InflVar(eui, unInfl, "glreg", "plural", inflVar, cat, cit));
                    }
                    else if (unInfl.EndsWith("on", StringComparison.Ordinal))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 2) + "a";
                        inflValues_.Add(new InflVar(eui, unInfl, "glreg", "plural", inflVar, cat, cit));
                    }
                    else if (unInfl.EndsWith("sis", StringComparison.Ordinal))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 2) + "es";
                        inflValues_.Add(new InflVar(eui, unInfl, "glreg", "plural", inflVar, cat, cit));
                    }
                    else if (unInfl.EndsWith("is", StringComparison.Ordinal))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "des";
                        inflValues_.Add(new InflVar(eui, unInfl, "glreg", "plural", inflVar, cat, cit));
                    }
                    else if (unInfl.EndsWith("men", StringComparison.Ordinal))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 2) + "ina";
                        inflValues_.Add(new InflVar(eui, unInfl, "glreg", "plural", inflVar, cat, cit));
                    }
                    else if (unInfl.EndsWith("ex", StringComparison.Ordinal))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 2) + "ices";
                        inflValues_.Add(new InflVar(eui, unInfl, "glreg", "plural", inflVar, cat, cit));
                    }
                    else if (unInfl.EndsWith("x", StringComparison.Ordinal))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ces";
                        inflValues_.Add(new InflVar(eui, unInfl, "glreg", "plural", inflVar, cat, cit));
                    }
                    else
                    {
                        Console.Error.WriteLine("** Err@glreg violation: " + eui + "|" + unInfl + "|" + cat);
                    }

                    agrValues_.Add(new Agreement(eui, inflVar, cat, "count(thr_plur)", unInfl, cit));
                }
                else if (variant.StartsWith("metareg", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(singular))
                    {
                        inflValues_.Add(singular);
                    }

                    if (!agrValues_.Contains(singAgr))
                    {
                        agrValues_.Add(singAgr);
                    }

                    inflVar = unInfl + "s";
                    InflVar plural = new InflVar(eui, unInfl, "metareg", "plural", inflVar, cat, cit);

                    plural.SetUnique(IsUnique(inflValues_, plural));
                    inflValues_.Add(plural);
                    agrValues_.Add(new Agreement(eui, inflVar, cat, "count(thr_plur)", unInfl, cit));

                    inflVar = unInfl + "'s";
                    InflVar plural2 = new InflVar(eui, unInfl, "metareg", "plural", inflVar, cat, cit);

                    plural2.SetUnique(IsUnique(inflValues_, plural2));
                    inflValues_.Add(plural2);
                    agrValues_.Add(new Agreement(eui, inflVar, cat, "count(thr_plur)", unInfl, cit));
                }
                else if (variant.StartsWith("irreg", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(singular))
                    {
                        inflValues_.Add(singular);
                    }

                    if (!agrValues_.Contains(singAgr))
                    {
                        agrValues_.Add(singAgr);
                    }

                    int index1 = variant.IndexOf("|", StringComparison.Ordinal);
                    int index2 = variant.IndexOf("|", index1 + 1, StringComparison.Ordinal);
                    int index3 = variant.IndexOf("|", index2 + 1, StringComparison.Ordinal);
                    string type = variant.Substring(0, index1);
                    string baseStr = variant.Substring(index1 + 1, index2 - (index1 + 1));
                    string pluralStr = variant.Substring(index2 + 1, index3 - (index2 + 1));

                    if (unInfl.Equals(baseStr) == true)
                    {
                        if (pluralStr.Length > 0)
                        {
                            InflVar plural = new InflVar(eui, unInfl, type, "plural", pluralStr, cat, cit);

                            plural.SetUnique(IsUnique(inflValues_, plural));
                            inflValues_.Add(plural);
                            agrValues_.Add(new Agreement(eui, pluralStr, cat, "count(thr_plur)", unInfl, cit));
                        }
                    }
                }
                else if (variant.StartsWith("sing", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(singular))
                    {
                        inflValues_.Add(singular);
                    }

                    if (!agrValues_.Contains(singAgr))
                    {
                        agrValues_.Add(singAgr);
                    }
                }
                else if (variant.StartsWith("plur", StringComparison.Ordinal))
                {
                    InflVar plural = new InflVar(eui, unInfl, "plur", "plural", unInfl, cat, cit);


                    plural.SetUnique(IsUnique(inflValues_, plural));
                    inflValues_.Add(plural);
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "count(thr_plur)", unInfl, cit));
                }
                else if (variant.StartsWith("inv", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(singular))
                    {
                        inflValues_.Add(singular);
                    }

                    if (!agrValues_.Contains(singAgr))
                    {
                        agrValues_.Add(singAgr);
                    }

                    InflVar plural = new InflVar(eui, unInfl, "inv", "plural", unInfl, cat, cit);

                    plural.SetUnique(IsUnique(inflValues_, plural));
                    inflValues_.Add(plural);
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "count(thr_plur)", unInfl, cit));
                }
                else if (variant.StartsWith("uncount", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(singular))
                    {
                        inflValues_.Add(singular);
                    }

                    agrValues_.Add(new Agreement(eui, unInfl, cat, "uncount(thr_sing)", unInfl, cit));
                }
                else if (variant.StartsWith("groupuncount", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(singular))
                    {
                        inflValues_.Add(singular);
                    }

                    agrValues_.Add(new Agreement(eui, unInfl, cat, "uncount(thr_sing)", unInfl, cit));

                    InflVar plural = new InflVar(eui, unInfl, "groupuncount", "plural", unInfl, cat, cit);

                    plural.SetUnique(IsUnique(inflValues_, plural));
                    inflValues_.Add(plural);
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "uncount(thr_plur)", unInfl, cit));
                }
                else if (variant.StartsWith("group(", StringComparison.Ordinal))
                {
                    if (!inflValues_.Contains(singular))
                    {
                        inflValues_.Add(singular);
                    }

                    if (!agrValues_.Contains(singAgr))
                    {
                        agrValues_.Add(singAgr);
                    }

                    InflVar plur = new InflVar(eui, unInfl, variant, "plural", unInfl, cat, cit);

                    plur.SetUnique(IsUnique(inflValues_, plur));

                    if ((IsUnique(inflValues_, plur) == true) && (!inflValues_.Contains(plur)))
                    {
                        inflValues_.Add(plur);
                        agrValues_.Add(new Agreement(eui, unInfl, cat, "count(thr_plur)", unInfl, cit));
                    }


                    string argument = variant.Substring(6, (variant.Length - 1) - 6);
                    if (argument == "reg")
                    {
                        char lastChar = GetLastChar(unInfl);
                        char last2Char = GetLast2Char(unInfl);
                        string last2CharStr = (new char?(last2Char)).ToString();
                        InflVar plural = null;
                        if ((lastChar == 'y') && (consonants_.Contains(last2CharStr)))
                        {
                            inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ies";
                            plural = new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit);
                        }
                        else if ((unInfl.EndsWith("s", StringComparison.Ordinal)) ||
                                 (unInfl.EndsWith("z", StringComparison.Ordinal)) ||
                                 (unInfl.EndsWith("x", StringComparison.Ordinal)) ||
                                 (unInfl.EndsWith("ch", StringComparison.Ordinal)) ||
                                 (unInfl.EndsWith("sh", StringComparison.Ordinal)))
                        {
                            inflVar = unInfl + "es";
                            plural = new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit);
                        }
                        else
                        {
                            inflVar = unInfl + "s";
                            plural = new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit);
                        }

                        plural.SetUnique(IsUnique(inflValues_, plural));
                        inflValues_.Add(plural);
                        agrValues_.Add(new Agreement(eui, inflVar, cat, "count(thr_plur)", unInfl, cit));
                    }
                    else if (argument == "glreg")
                    {
                        if (unInfl.EndsWith("us", StringComparison.Ordinal))
                        {
                            inflVar = unInfl.Substring(0, unInfl.Length - 2) + "i";
                            inflValues_.Add(new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit));
                        }
                        else if (unInfl.EndsWith("ma", StringComparison.Ordinal))
                        {
                            inflVar = unInfl + "ta";
                            inflValues_.Add(new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit));
                        }
                        else if (unInfl.EndsWith("a", StringComparison.Ordinal))
                        {
                            inflVar = unInfl + "e";
                            inflValues_.Add(new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit));
                        }
                        else if (unInfl.EndsWith("um", StringComparison.Ordinal))
                        {
                            inflVar = unInfl.Substring(0, unInfl.Length - 2) + "a";
                            inflValues_.Add(new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit));
                        }
                        else if (unInfl.EndsWith("on", StringComparison.Ordinal))
                        {
                            inflVar = unInfl.Substring(0, unInfl.Length - 2) + "a";
                            inflValues_.Add(new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit));
                        }
                        else if (unInfl.EndsWith("sis", StringComparison.Ordinal))
                        {
                            inflVar = unInfl.Substring(0, unInfl.Length - 2) + "es";
                            inflValues_.Add(new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit));
                        }
                        else if (unInfl.EndsWith("is", StringComparison.Ordinal))
                        {
                            inflVar = unInfl.Substring(0, unInfl.Length - 1) + "des";
                            inflValues_.Add(new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit));
                        }
                        else if (unInfl.EndsWith("men", StringComparison.Ordinal))
                        {
                            inflVar = unInfl.Substring(0, unInfl.Length - 2) + "ina";
                            inflValues_.Add(new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit));
                        }
                        else if (unInfl.EndsWith("ex", StringComparison.Ordinal))
                        {
                            inflVar = unInfl.Substring(0, unInfl.Length - 2) + "ices";
                            inflValues_.Add(new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit));
                        }
                        else if (unInfl.EndsWith("x", StringComparison.Ordinal))
                        {
                            inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ces";
                            inflValues_.Add(new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit));
                        }
                        else
                        {
                            Console.Error.WriteLine("** Err@glreg violation: " + eui + "|" + unInfl + "|" + cat);
                        }

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "count(thr_plur)", unInfl, cit));
                    }
                    else if (argument == "metareg")
                    {
                        inflVar = unInfl + "s";
                        InflVar plural = new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit);

                        plural.SetUnique(IsUnique(inflValues_, plural));
                        inflValues_.Add(plural);
                        agrValues_.Add(new Agreement(eui, inflVar, cat, "count(thr_plur)", unInfl, cit));

                        inflVar = unInfl + "'s";
                        InflVar plural2 = new InflVar(eui, unInfl, variant, "plural", inflVar, cat, cit);

                        plural2.SetUnique(IsUnique(inflValues_, plural2));
                        inflValues_.Add(plural2);
                        agrValues_.Add(new Agreement(eui, inflVar, cat, "count(thr_plur)", unInfl, cit));
                    }
                    else if (argument.StartsWith("irreg", StringComparison.Ordinal))
                    {
                        int index1 = argument.IndexOf("|", StringComparison.Ordinal);
                        int index2 = argument.IndexOf("|", index1 + 1, StringComparison.Ordinal);
                        int index3 = argument.IndexOf("|", index2 + 1, StringComparison.Ordinal);
                        string tagStr = argument.Substring(0, index1);
                        string baseStr = argument.Substring(index1 + 1, index2 - (index1 + 1));
                        string pluralStr = argument.Substring(index2 + 1, index3 - (index2 + 1));

                        if (unInfl.Equals(baseStr) == true)
                        {
                            if (pluralStr.Length > 0)
                            {
                                InflVar plural = new InflVar(eui, unInfl, variant, "plural", pluralStr, cat, cit);

                                plural.SetUnique(IsUnique(inflValues_, plural));
                                inflValues_.Add(plural);
                                agrValues_.Add(new Agreement(eui, pluralStr, cat, "count(thr_plur)", unInfl, cit));
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("-- Warning: InflVarsAndAgreements.GetNounInflVarsAndAgreements()" + variant);
                }
            }
        }


        private void GetAdjInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));

            InflVar positive = new InflVar(eui, unInfl, "positive", unInfl, cat, cit);

            inflValues_.Add(positive);


            List<string> variants = lexRecord_.GetCatEntry().GetAdjEntry().GetVariants();
            string inflVar = "";
            for (int i = 0; i < variants.Count; i++)
            {
                string variant = (string) variants[i];
                if (variant.StartsWith("irreg", StringComparison.Ordinal))
                {
                    int index1 = variant.IndexOf("|", StringComparison.Ordinal);
                    int index2 = variant.IndexOf("|", index1 + 1, StringComparison.Ordinal);
                    int index3 = variant.IndexOf("|", index2 + 1, StringComparison.Ordinal);
                    int index4 = variant.IndexOf("|", index3 + 1, StringComparison.Ordinal);
                    string type = variant.Substring(0, index1);
                    string baseStr = variant.Substring(index1 + 1, index2 - (index1 + 1));
                    string compStr = variant.Substring(index2 + 1, index3 - (index2 + 1));
                    string superStr = variant.Substring(index3 + 1, index4 - (index3 + 1));
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "positive", unInfl, cit));


                    if (unInfl.Equals(baseStr) == true)
                    {
                        if (compStr.Length > 0)
                        {
                            InflVar comparative = new InflVar(eui, unInfl, type, "comparative", compStr, cat, cit);

                            comparative.SetUnique(IsUnique(inflValues_, comparative));

                            inflValues_.Add(comparative);
                        }


                        if (superStr.Length > 0)
                        {
                            InflVar superlative = new InflVar(eui, unInfl, type, "superlative", superStr, cat, cit);

                            superlative.SetUnique(IsUnique(inflValues_, superlative));

                            inflValues_.Add(superlative);
                            agrValues_.Add(new Agreement(eui, superStr, cat, "superlative", unInfl, cit));
                        }
                    }
                }
                else if (variant.StartsWith("regd", StringComparison.Ordinal))
                {
                    string type = "regd";
                    char lastChar = GetLastChar(unInfl);
                    char last2Char = GetLast2Char(unInfl);
                    string lastCharStr = (new char?(lastChar)).ToString();
                    string last2CharStr = (new char?(last2Char)).ToString();

                    if ((!consonants_.Contains(lastCharStr)) || (!vowels_.Contains(last2CharStr)))
                    {
                        Console.Error.WriteLine("** Err@regd violation: " + eui + "|" + unInfl + "|" + cat);
                    }

                    agrValues_.Add(new Agreement(eui, unInfl, cat, "positive", unInfl, cit));


                    inflVar = unInfl + lastChar + "er";
                    inflValues_.Add(new InflVar(eui, unInfl, type, "comparative", inflVar, cat, cit));

                    agrValues_.Add(new Agreement(eui, inflVar, cat, "comparative", unInfl, cit));


                    inflVar = unInfl + lastChar + "est";
                    inflValues_.Add(new InflVar(eui, unInfl, type, "superlative", inflVar, cat, cit));

                    agrValues_.Add(new Agreement(eui, inflVar, cat, "superlative", unInfl, cit));
                }
                else if (variant.StartsWith("reg", StringComparison.Ordinal))
                {
                    string type = "reg";
                    char lastChar = GetLastChar(unInfl);
                    char last2Char = GetLast2Char(unInfl);
                    string last2CharStr = (new char?(last2Char)).ToString();
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "positive", unInfl, cit));

                    if ((lastChar == 'y') && (consonants_.Contains(last2CharStr)))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ier";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "comparative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "comparative", unInfl, cit));


                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "iest";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "superlative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "superlative", unInfl, cit));
                    }
                    else if (lastChar == 'e')
                    {
                        inflVar = unInfl + "r";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "comparative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "comparative", unInfl, cit));


                        inflVar = unInfl + "st";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "superlative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "superlative", unInfl, cit));
                    }
                    else
                    {
                        inflVar = unInfl + "er";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "comparative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "comparative", unInfl, cit));


                        inflVar = unInfl + "est";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "superlative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "superlative", unInfl, cit));
                    }
                }
                else if (variant.StartsWith("inv;periph", StringComparison.Ordinal))
                {
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "positive;periph", unInfl, cit));
                }
                else
                {
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "positive", unInfl, cit));
                }
            }
        }


        private void GetAdvInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));

            InflVar positive = new InflVar(eui, unInfl, "positive", unInfl, cat, cit);

            inflValues_.Add(positive);


            List<string> variants = lexRecord_.GetCatEntry().GetAdvEntry().GetVariants();
            string inflVar = "";
            for (int i = 0; i < variants.Count; i++)
            {
                string variant = (string) variants[i];
                if (variant.StartsWith("irreg", StringComparison.Ordinal))
                {
                    int index1 = variant.IndexOf("|", StringComparison.Ordinal);
                    int index2 = variant.IndexOf("|", index1 + 1, StringComparison.Ordinal);
                    int index3 = variant.IndexOf("|", index2 + 1, StringComparison.Ordinal);
                    int index4 = variant.IndexOf("|", index3 + 1, StringComparison.Ordinal);
                    string type = variant.Substring(0, index1);
                    string baseStr = variant.Substring(index1 + 1, index2 - (index1 + 1));
                    string comparative = variant.Substring(index2 + 1, index3 - (index2 + 1));
                    string superlative = variant.Substring(index3 + 1, index4 - (index3 + 1));
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "positive", unInfl, cit));

                    if (unInfl.Equals(baseStr) == true)
                    {
                        if (comparative.Length > 0)
                        {
                            inflValues_.Add(new InflVar(eui, unInfl, type, "comparative", comparative, cat, cit));
                        }


                        if (superlative.Length > 0)
                        {
                            inflValues_.Add(new InflVar(eui, unInfl, type, "superlative", superlative, cat, cit));
                        }
                    }


                    if (comparative.Length > 0)
                    {
                        agrValues_.Add(new Agreement(eui, comparative, cat, "comparative", unInfl, cit));
                    }

                    if (superlative.Length > 0)
                    {
                        agrValues_.Add(new Agreement(eui, superlative, cat, "superlative", unInfl, cit));
                    }
                }
                else if (variant.StartsWith("reg", StringComparison.Ordinal))
                {
                    string type = "reg";
                    char lastChar = GetLastChar(unInfl);
                    char last2Char = GetLast2Char(unInfl);
                    string last2CharStr = (new char?(last2Char)).ToString();
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "positive", unInfl, cit));

                    if ((lastChar == 'y') && (consonants_.Contains(last2CharStr)))
                    {
                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "ier";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "comparative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "comparative", unInfl, cit));


                        inflVar = unInfl.Substring(0, unInfl.Length - 1) + "iest";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "superlative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "superlative", unInfl, cit));
                    }
                    else if (lastChar == 'e')
                    {
                        inflVar = unInfl + "r";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "comparative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "comparative", unInfl, cit));


                        inflVar = unInfl + "st";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "superlative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "superlative", unInfl, cit));
                    }
                    else
                    {
                        inflVar = unInfl + "er";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "comparative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "comparative", unInfl, cit));


                        inflVar = unInfl + "est";
                        inflValues_.Add(new InflVar(eui, unInfl, type, "superlative", inflVar, cat, cit));

                        agrValues_.Add(new Agreement(eui, inflVar, cat, "superlative", unInfl, cit));
                    }
                }
                else if (variant.StartsWith("inv;periph", StringComparison.Ordinal))
                {
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "positive;periph", unInfl, cit));
                }
                else
                {
                    agrValues_.Add(new Agreement(eui, unInfl, cat, "positive", unInfl, cit));
                }
            }
        }

        public static char GetLastChar(string @in)
        {
            int length = @in.Length;
            char @out = @in.ToLower()[length - 1];
            return @out;
        }

        public static char GetLast2Char(string @in)
        {
            char @out = ' ';
            int length = @in.Length;
            if (length >= 2)
            {
                @out = @in.ToLower()[length - 2];
            }

            return @out;
        }


        private void GetAuxInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));


            List<string> variants = lexRecord_.GetCatEntry().GetAuxEntry().GetVariant();
            for (int i = 0; i < variants.Count; i++)
            {
                string variant = (string) variants[i];
                int index = variant.IndexOf(";", StringComparison.Ordinal);
                string value = variant.Substring(0, index);
                int index1 = variant.IndexOf("(", index, StringComparison.Ordinal);
                int index2 = variant.IndexOf(":", index, StringComparison.Ordinal);
                string inflStr = variant.Substring(index + 1);
                string infl = MapToInflection(inflStr);
                inflValues_.Add(new InflVar(eui, unInfl, "reg", infl, value, cat, cit));

                agrValues_.Add(new Agreement(eui, value, cat, inflStr, unInfl, cit));
            }
        }


        private void GetModalInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));

            inflValues_.Add(new InflVar(eui, unInfl, "pres", unInfl, cat, cit));

            agrValues_.Add(new Agreement(eui, unInfl, cat, "pres", unInfl, cit));


            List<string> variants = lexRecord_.GetCatEntry().GetModalEntry().GetVariant();
            for (int i = 0; i < variants.Count; i++)
            {
                string variant = (string) variants[i];
                int index = variant.IndexOf(";", StringComparison.Ordinal);
                string value = variant.Substring(0, index);
                int index1 = variant.IndexOf("(", index, StringComparison.Ordinal);
                int index2 = variant.IndexOf(":", index, StringComparison.Ordinal);
                string inflStr = variant.Substring(index + 1);
                string infl = MapToInflection(inflStr);
                inflValues_.Add(new InflVar(eui, unInfl, "reg", infl, value, cat, cit));

                agrValues_.Add(new Agreement(eui, value, cat, inflStr, unInfl, cit));
            }
        }


        private string MapToInflection(string @in)
        {
            char[] delim = " (,):".ToCharArray();
            string[] buf = @in.Split(delim);
            string @out = "";
            foreach (string curToken in buf)
            {
                if (curToken == "past")
                {
                    @out = @out + "past";
                }
                else if (curToken == "pres")
                {
                    @out = @out + "pres";
                }
                else if (curToken == "past_part")
                {
                    @out = @out + "pastPart";
                }
                else if (curToken == "pres_part")
                {
                    @out = @out + "presPart";
                }
                else if (curToken == "infinitive")
                {
                    @out = @out + "infinitive";
                }
                else if (curToken == "negative")
                {
                    @out = @out + "Neg";
                }
                else if (curToken == "fst_sing")
                {
                    @out = @out + "1s";
                }
                else if (curToken == "fst_plur")
                {
                    @out = @out + "1p";
                }
                else if (curToken == "second")
                {
                    @out = @out + "2";
                }
                else if (curToken == "sec_sing")
                {
                    @out = @out + "2s";
                }
                else if (curToken == "sec_plur")
                {
                    @out = @out + "2p";
                }
                else if (curToken == "third")
                {
                    @out = @out + "3";
                }
                else if (curToken == "thr_sing")
                {
                    @out = @out + "3s";
                }
                else if (curToken == "thr_plur")
                {
                    @out = @out + "3p";
                }
            }

            int index = @out.IndexOf("1s1p", StringComparison.Ordinal);
            if (index > 0)
            {
                @out = @out.Substring(0, index + 1) + @out.Substring(index + 4);
            }

            if (!inflections_.Contains(@out))
            {
                Console.WriteLine("** Error: inflection '" + @out + "' is illegal");
                @out = "Error";
            }

            return @out;
        }

        private void GetPronInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));


            List<string> variants = lexRecord_.GetCatEntry().GetPronEntry().GetVariants();
            string agreement = "";
            for (int i = 0; i < variants.Count; i++)
            {
                agreement = (string) variants[i];
                agrValues_.Add(new Agreement(eui, unInfl, cat, agreement, unInfl, cit));
            }
        }


        private void GetDetInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));


            string agreement = lexRecord_.GetCatEntry().GetDetEntry().GetVariants();
            if (agreement == "singuncount")
            {
                agrValues_.Add(new Agreement(eui, unInfl, cat, "sing", unInfl, cit));

                agrValues_.Add(new Agreement(eui, unInfl, cat, "uncount", unInfl, cit));
            }
            else if (agreement == "pluruncount")
            {
                agrValues_.Add(new Agreement(eui, unInfl, cat, "plur", unInfl, cit));

                agrValues_.Add(new Agreement(eui, unInfl, cat, "uncount", unInfl, cit));
            }
            else
            {
                agrValues_.Add(new Agreement(eui, unInfl, cat, agreement, unInfl, cit));
            }
        }


        private void GetPrepInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));


            agrValues_.Add(new Agreement(eui, unInfl, cat, "", unInfl, cit));
        }


        private void GetComplInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));


            agrValues_.Add(new Agreement(eui, unInfl, cat, "", unInfl, cit));
        }


        private void GetConjInflVarsAndAgreements(string unInfl, string eui, string cat, string cit)
        {
            inflValues_.Add(new InflVar(eui, unInfl, "base", unInfl, cat, cit));


            agrValues_.Add(new Agreement(eui, unInfl, cat, "", unInfl, cit));
        }


        private bool IsUnique(List<InflVar> inflVars, InflVar inflVar)
        {
            bool isUnique = true;
            for (int i = 0; i < inflVars.Count; i++)
            {
                InflVar cur = (InflVar) inflVars[i];
                if ((inflVar.GetEui() == null) || (cur.GetEui() == null))
                {
                    if ((inflVar.GetVar().Equals(cur.GetVar())) &&
                        (inflVar.GetInflection().Equals(cur.GetInflection())) &&
                        (inflVar.GetCat().Equals(cur.GetCat())) && (inflVar.GetCit().Equals(cur.GetCit())) &&
                        (inflVar.GetUnInfl().Equals(cur.GetUnInfl())))
                    {
                        isUnique = false;
                        break;
                    }
                }
                else if ((inflVar.GetVar().Equals(cur.GetVar())) &&
                         (inflVar.GetInflection().Equals(cur.GetInflection())) &&
                         (inflVar.GetCat().Equals(cur.GetCat())) && (inflVar.GetCit().Equals(cur.GetCit())) &&
                         (inflVar.GetEui().Equals(cur.GetEui())) && (inflVar.GetUnInfl().Equals(cur.GetUnInfl())))
                {
                    isUnique = false;
                    break;
                }
            }

            return isUnique;
        }

        private LexRecord lexRecord_ = null;
        private List<InflVar> inflValues_ = new List<InflVar>();
        private List<Agreement> agrValues_ = new List<Agreement>();
        private static HashSet<string> inflections_ = new HashSet<string>();
        public static HashSet<string> consonants_ = new HashSet<string>();
        public static HashSet<string> vowels_ = new HashSet<string>();
        private static HashSet<string> eioySets_ = new HashSet<string>();

        static InflVarsAndAgreements()
        {
            inflections_.Add("base");
            inflections_.Add("comparative");
            inflections_.Add("superlative");
            inflections_.Add("plural");
            inflections_.Add("presPart");
            inflections_.Add("past");
            inflections_.Add("pastPart");
            inflections_.Add("pres3s");
            inflections_.Add("positive");
            inflections_.Add("singular");
            inflections_.Add("infinitive");
            inflections_.Add("pres123p");
            inflections_.Add("pastNeg");
            inflections_.Add("pres123pNeg");
            inflections_.Add("pres1s");
            inflections_.Add("past1p23pNeg");
            inflections_.Add("past1p23p");
            inflections_.Add("past1s3sNeg");
            inflections_.Add("pres1p23p");
            inflections_.Add("pres1p23pNeg");
            inflections_.Add("past1s3s");
            inflections_.Add("pres");
            inflections_.Add("pres3sNeg");
            inflections_.Add("presNeg");
            vowels_.Add("a");
            vowels_.Add("e");
            vowels_.Add("i");
            vowels_.Add("o");
            vowels_.Add("u");
            consonants_.Add("b");
            consonants_.Add("c");
            consonants_.Add("d");
            consonants_.Add("f");
            consonants_.Add("g");
            consonants_.Add("h");
            consonants_.Add("j");
            consonants_.Add("k");
            consonants_.Add("l");
            consonants_.Add("m");
            consonants_.Add("n");
            consonants_.Add("p");
            consonants_.Add("q");
            consonants_.Add("r");
            consonants_.Add("s");
            consonants_.Add("t");
            consonants_.Add("v");
            consonants_.Add("w");
            consonants_.Add("x");
            consonants_.Add("y");
            consonants_.Add("z");
            eioySets_.Add("e");
            eioySets_.Add("i");
            eioySets_.Add("o");
            eioySets_.Add("y");
        }
    }
}