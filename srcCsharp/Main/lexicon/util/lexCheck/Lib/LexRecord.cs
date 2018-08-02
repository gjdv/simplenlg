using System;
using System.Collections.Generic;
using SimpleNLG.Main.framework;

namespace SimpleNLG.Main.lexicon.util.lexCheck.Lib
{
    public class LexRecord
    {
        public LexRecord()
        {
            base_ = null;
            spellingVars_ = new List<string>();
            eui_ = null;
            category_ = null;
            catEntry_ = null;
            acronyms_ = new List<string>();
            abbreviations_ = new List<string>();
            annotations_ = new List<string>();
            signature_ = null;
            end_ = "}";
        }

        public virtual void SetLexRecord(LexRecord lexRecord)
        {
            base_ = lexRecord.GetBase();
            spellingVars_ = lexRecord.GetSpellingVars();
            eui_ = lexRecord.GetEui();
            category_ = lexRecord.GetCategory();
            catEntry_ = lexRecord.GetCatEntry();
            acronyms_ = lexRecord.GetAcronyms();
            abbreviations_ = lexRecord.GetAbbreviations();
            annotations_ = lexRecord.GetAnnotations();
            signature_ = lexRecord.GetSignature();
            end_ = lexRecord.GetEnd();
        }

        public virtual void Reset()
        {
            base_ = "";
            spellingVars_ = new List<string>();
            eui_ = null;
            category_ = "";
            catEntry_ = null;
            acronyms_ = new List<string>();
            abbreviations_ = new List<string>();
            annotations_ = new List<string>();
            signature_ = null;
            end_ = "}";
        }

        public virtual string GetXml()
        {
            return GetXml(0);
        }

        public static string GetXmlRootBeginTag()
        {
            string xml = "<" + xmlRootTag_ + ">";
            return xml;
        }

        public static string GetXmlRootEndTag()
        {
            string xml = "</" + xmlRootTag_ + ">";
            return xml;
        }

        public virtual string GetXml(int indLevel)
        {
            bool convertFlag = true;
            string xml = "";
            xml = XmlLib.AddToXml(xml, "<lexRecord>", indLevel);
            xml = XmlLib.AddToXml(xml, "<base>", "</base>", base_, indLevel + 1, convertFlag);
            xml = XmlLib.AddToXml(xml, "<eui>", "</eui>", eui_, indLevel + 1, convertFlag);
            xml = XmlLib.AddToXml(xml, "<cat>", "</cat>", category_, indLevel + 1, convertFlag);
            xml = XmlLib.AddToXml(xml, "<spellingVars>", "</spellingVars>", spellingVars_, indLevel + 1,
                convertFlag);
            xml = xml + InflVarsAndAgreements.GetXml(this);
            xml = xml + catEntry_.GetXml(category_);
            xml = XmlLib.AddToXml(xml, "<acronyms>", "</acronyms>", acronyms_, indLevel + 1, convertFlag);
            xml = XmlLib.AddToXml(xml, "<abbreviations>", "</abbreviations>", abbreviations_, indLevel + 1,
                convertFlag);
            xml = XmlLib.AddToXml(xml, "<annotations>", "</annotations>", annotations_, indLevel + 1, convertFlag);
            xml = XmlLib.AddToXml(xml, "<signature>", "</signature>", signature_, indLevel + 1, convertFlag);
            xml = XmlLib.AddToXml(xml, "</lexRecord>", indLevel);
            return xml;
        }

        public virtual string GetText()
        {
            string text = "";
            text = TextLib.AddToText(text, "{base=", base_, 0);
            text = TextLib.AddToText(text, "spelling_variant=", spellingVars_, 0);
            text = TextLib.AddToText(text, "entry=", eui_, 0);
            text = TextLib.AddToText(text, "cat=", category_, 1);
            text = text + catEntry_.GetText(category_);
            text = TextLib.AddToText(text, "acronym_of=", acronyms_, 1);
            text = TextLib.AddToText(text, "abbreviation_of=", abbreviations_, 1);
            text = TextLib.AddToText(text, "annotation=", annotations_, 0);
            text = TextLib.AddToText(text, "signature=", signature_, 0);
            text = TextLib.AddToText(text, "", end_, 0);
            return text;
        }

        public virtual string GetReleaseFormatText()
        {
            string text = "";
            text = TextLib.AddToText(text, "{base=", base_, 0);
            text = TextLib.AddToText(text, "spelling_variant=", spellingVars_, 0);
            text = TextLib.AddToText(text, "entry=", eui_, 0);
            text = TextLib.AddToText(text, "cat=", category_, 1);
            text = text + catEntry_.GetText(category_);
            text = TextLib.AddToText(text, "acronym_of=", acronyms_, 1);
            text = TextLib.AddToText(text, "abbreviation_of=", abbreviations_, 1);
            text = TextLib.AddToText(text, "", end_, 0);
            return text;
        }

        public virtual string GetDbFormatText(string cDate)
        {
            string SP = "|";
            string cSig = signature_;
            string mSig = signature_;
            string aSig = "browne";
            int lastAct = 0;
            string text = "";
            text = eui_ + SP + base_ + SP + category_ + SP + cSig + SP + mSig + SP + aSig + SP +
                   lastAct + SP + cDate + SP + cDate + SP + cDate + SP;
            text = TextLib.AddToText(text, "{base=", base_, 0);
            text = TextLib.AddToText(text, "spelling_variant=", spellingVars_, 0);
            text = TextLib.AddToText(text, "entry=", eui_, 0);
            text = TextLib.AddToText(text, "cat=", category_, 1);
            text = text + catEntry_.GetText(category_);
            text = TextLib.AddToText(text, "acronym_of=", acronyms_, 1);
            text = TextLib.AddToText(text, "abbreviation_of=", abbreviations_, 1);
            text = TextLib.AddToText(text, "", end_, 0);
            return text;
        }

        public virtual InflVarsAndAgreements GetInflVarsAndAgreements()
        {
            return new InflVarsAndAgreements(this);
        }

        public virtual void Print()
        {
            Console.Write(GetText());
            Console.Write(GetXml());
        }

        public virtual void PrintReleaseFormatText()
        {
            Console.Write(GetReleaseFormatText());
        }

        public virtual void PrintDbFormatText(string cDate)
        {
            Console.Write(GetDbFormatText(cDate));
        }

        public virtual void PrintText()
        {
            Console.Write(GetText());
        }

        public virtual void PrintXml()
        {
            Console.Write(GetXml());
        }

        public virtual string GetBase()
        {
            return base_;
        }

        public virtual string GetEui()
        {
            return eui_;
        }

        public virtual List<string> GetSpellingVars()
        {
            return spellingVars_;
        }

        public virtual List<string> GetAcronyms()
        {
            return acronyms_;
        }

        public virtual List<string> GetAbbreviations()
        {
            return abbreviations_;
        }

        public virtual List<string> GetNominalizations()
        {
            List<string> nominalizations = new List<string>();
            if (category_.Equals("noun"))
            {
                nominalizations = catEntry_.GetNounEntry().GetNominalization();
            }
            else if (category_.Equals("verb"))
            {
                nominalizations = catEntry_.GetVerbEntry().GetNominalization();
            }
            else if (category_.Equals("adj"))
            {
                nominalizations = catEntry_.GetAdjEntry().GetNominalization();
            }

            return nominalizations;
        }

        public virtual List<string> GetVariants()
        {
            List<string> variants = new List<string>();
            if (category_.Equals("noun"))
            {
                variants = catEntry_.GetNounEntry().GetVariants();
            }
            else if (category_.Equals("verb"))
            {
                variants = catEntry_.GetVerbEntry().GetVariants();
            }
            else if (category_.Equals("adj"))
            {
                variants = catEntry_.GetAdjEntry().GetVariants();
            }
            else if (category_.Equals("adv"))
            {
                variants = catEntry_.GetAdvEntry().GetVariants();
            }
            else if (category_.Equals("pron"))
            {
                variants = catEntry_.GetPronEntry().GetVariants();
            }
            else if (category_.Equals("modal"))
            {
                variants = catEntry_.GetModalEntry().GetVariant();
            }
            else if (category_.Equals("aux"))
            {
                variants = catEntry_.GetAuxEntry().GetVariant();
            }

            return variants;
        }

        public virtual List<string> GetAnnotations()
        {
            return annotations_;
        }

        public virtual string GetSignature()
        {
            return signature_;
        }

        public virtual string GetCategory()
        {
            return category_;
        }
        public LexicalCategory GetSimpleNLGCategory(LexRecord record)
        {
            string cat = GetCategory();
            LexicalCategory.LexicalCategoryEnum catEnum = LexicalCategory.LexicalCategoryEnum.ANY;
            if(cat == null)
                catEnum = LexicalCategory.LexicalCategoryEnum.ANY;
            else if (cat.Equals("noun", StringComparison.CurrentCultureIgnoreCase))
                catEnum = LexicalCategory.LexicalCategoryEnum.NOUN;
            else if (cat.Equals("verb", StringComparison.CurrentCultureIgnoreCase))
                catEnum = LexicalCategory.LexicalCategoryEnum.VERB;
            else if (cat.Equals("aux", StringComparison.CurrentCultureIgnoreCase)
                     && record.GetBase().Equals("be", StringComparison.CurrentCultureIgnoreCase)) // return aux "be"
                // as a VERB
                // not needed for other aux "have" and "do", they have a verb entry
                catEnum = LexicalCategory.LexicalCategoryEnum.VERB;
            else if (cat.Equals("adj", StringComparison.CurrentCultureIgnoreCase))
                catEnum = LexicalCategory.LexicalCategoryEnum.ADJECTIVE;
            else if (cat.Equals("adv", StringComparison.CurrentCultureIgnoreCase))
                catEnum = LexicalCategory.LexicalCategoryEnum.ADVERB;
            else if (cat.Equals("pron", StringComparison.CurrentCultureIgnoreCase))
                catEnum = LexicalCategory.LexicalCategoryEnum.PRONOUN;
            else if (cat.Equals("det", StringComparison.CurrentCultureIgnoreCase))
                catEnum = LexicalCategory.LexicalCategoryEnum.DETERMINER;
            else if (cat.Equals("prep", StringComparison.CurrentCultureIgnoreCase))
                catEnum = LexicalCategory.LexicalCategoryEnum.PREPOSITION;
            else if (cat.Equals("conj", StringComparison.CurrentCultureIgnoreCase))
                catEnum = LexicalCategory.LexicalCategoryEnum.CONJUNCTION;
            else if (cat.Equals("compl", StringComparison.CurrentCultureIgnoreCase))
                catEnum = LexicalCategory.LexicalCategoryEnum.COMPLEMENTISER;
            else if (cat.Equals("modal", StringComparison.CurrentCultureIgnoreCase))
                catEnum = LexicalCategory.LexicalCategoryEnum.MODAL;

            return new LexicalCategory(catEnum);
        }

        public virtual string GetEnd()
        {
            return end_;
        }

        public virtual void SetBase(string @base)
        {
            base_ = @base;
        }

        public virtual void SetEui(string eui)
        {
            eui_ = eui;
        }

        public virtual void SetSpellingVar(string spellingVar)
        {
            spellingVars_.Add(spellingVar);
        }

        public virtual void SetSpellingVars(List<string> spellingVars)
        {
            spellingVars_ = spellingVars;
        }

        public virtual void SetAcronym(string acronym)
        {
            acronyms_.Add(acronym);
        }

        public virtual void SetAbbreviation(string abbreviation)
        {
            abbreviations_.Add(abbreviation);
        }

        public virtual void SetAnnotation(string annotation)
        {
            annotations_.Add(annotation);
        }

        public virtual void SetAcronyms(List<string> acronyms)
        {
            acronyms_ = acronyms;
        }

        public virtual void SetAbbreviations(List<string> abbreviations)
        {
            abbreviations_ = abbreviations;
        }

        public virtual void SetNominalizations(List<string> nominalizations)
        {
            if (category_.Equals("noun"))
            {
                catEntry_.GetNounEntry().SetNominalization(nominalizations);
            }
            else if (category_.Equals("verb"))
            {
                catEntry_.GetVerbEntry().SetNominalization(nominalizations);
            }
            else if (category_.Equals("adj"))
            {
                catEntry_.GetAdjEntry().SetNominalization(nominalizations);
            }
        }

        public virtual void SetVariants(List<string> variants)
        {
            if (category_.Equals("noun"))
            {
                catEntry_.GetNounEntry().SetVariants(variants);
            }
            else if (category_.Equals("verb"))
            {
                catEntry_.GetVerbEntry().SetVariants(variants);
            }
            else if (category_.Equals("adj"))
            {
                catEntry_.GetAdjEntry().SetVariants(variants);
            }
            else if (category_.Equals("adv"))
            {
                catEntry_.GetAdvEntry().SetVariants(variants);
            }
            else if (category_.Equals("pron"))
            {
                catEntry_.GetPronEntry().SetVariants(variants);
            }
            else if (category_.Equals("modal"))
            {
                catEntry_.GetModalEntry().SetVariant(variants);
            }
            else if (category_.Equals("aux"))
            {
                catEntry_.GetAuxEntry().SetVariant(variants);
            }
        }

        public virtual void SetAnnotations(List<string> annotations)
        {
            annotations_ = annotations;
        }

        public virtual void SetSignature(string signature)
        {
            signature_ = signature;
        }

        public virtual void SetCat(string category)
        {
            category_ = category;
            catEntry_ = new CatEntry(category_);
        }

        public virtual void SetCatEntry(CatEntry catEntry)
        {
            catEntry_ = catEntry;
        }

        public virtual void SetEnd(string end)
        {
            end_ = end;
        }

        public virtual void SetPrintXmlHeader(bool printXmlHeader)
        {
            printXmlHeader_ = printXmlHeader;
        }

        public virtual void SetXmlHeader(string xmlHeader)
        {
            xmlHeader_ = xmlHeader;
        }

        public static void SetXmlRootTag(string xmlRootTag)
        {
            xmlRootTag_ = xmlRootTag;
        }

        public virtual CatEntry GetCatEntry()
        {
            return catEntry_;
        }

        public static string GetXmlHeader()
        {
            return xmlHeader_;
        }

        public static void Main(string[] args)
        {
            LexRecord lexRec = new LexRecord();
            lexRec.SetBase("123");
            lexRec.SetCat("noun");
            lexRec.GetCatEntry().GetNounEntry().AddVariant("irreg|123||");
            lexRec.GetCatEntry().GetNounEntry().AddVariant("group(irreg|123|222|)");
            Console.WriteLine("----------------");
            Console.WriteLine(lexRec.GetText());
            Console.WriteLine("----------------");
            InflVarsAndAgreements inflVars = lexRec.GetInflVarsAndAgreements();
            List<InflVar> inflValues = inflVars.GetInflValues();
            for (int i = 0; i < inflValues.Count; i++)
            {
                InflVar inflectionVar = inflValues[i];
                Console.WriteLine(inflectionVar.GetVar() + "|" + inflectionVar.GetCat() + "|" +
                                  inflectionVar.GetInflection() + "|" + inflectionVar.GetUnInfl() + "|" +
                                  inflectionVar.GetCit() + "|" + inflectionVar.GetType());
            }
        }

        private string base_;
        private List<string> spellingVars_ = new List<string>();
        private string eui_;
        private string category_;
        private CatEntry catEntry_;
        private List<string> acronyms_ = new List<string>();
        private List<string> abbreviations_ = new List<string>();
        private List<string> annotations_ = new List<string>();
        private string signature_;
        private string end_;
        private bool printXmlHeader_ = true;
        private static string xmlHeader_ = "";
        private static string xmlRootTag_ = "lexRecords";
    }
}