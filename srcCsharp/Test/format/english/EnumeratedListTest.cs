/*
 * Ported to C# by Gert-Jan de Vries
 */

using NUnit.Framework;
using SimpleNLG.Main.format.english;
using SimpleNLG.Main.framework;
using SimpleNLG.Main.lexicon;
using SimpleNLG.Main.realiser.english;

namespace SimpleNLG.Test.format.english
{
    using DocumentElement = DocumentElement;
    using NLGFactory = NLGFactory;
    using Lexicon = Lexicon;
    using Realiser = Realiser;

    [TestFixture]
    public class EnumeratedListTest
    {
        [Test]
        public virtual void bulletList()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon;
            NLGFactory nlgFactory = new NLGFactory(lexicon);
            Realiser realiser = new Realiser(lexicon);
            realiser.Formatter = new HTMLFormatter();
            DocumentElement document = nlgFactory.createDocument("Document");
            DocumentElement paragraph = nlgFactory.createParagraph();
            DocumentElement list = nlgFactory.createList();
            DocumentElement item1 = nlgFactory.createListItem();
            DocumentElement item2 = nlgFactory.createListItem();


            DocumentElement sentence1 = nlgFactory.createSentence("this", "be", "the first sentence");
            DocumentElement sentence2 = nlgFactory.createSentence("this", "be", "the second sentence");
            item1.addComponent(sentence1);
            item2.addComponent(sentence2);
            list.addComponent(item1);
            list.addComponent(item2);
            paragraph.addComponent(list);
            document.addComponent(paragraph);
            string expectedOutput = "<h1>Document</h1>" + "<p>" + "<ul>" + "<li>This is the first sentence.</li>"
                                    + "<li>This is the second sentence.</li>" + "</ul>" + "</p>";

            string realisedOutput = realiser.realise(document).Realisation;


            Assert.AreEqual(expectedOutput, realisedOutput);
        }

        [Test]
        public virtual void enumeratedList()
        {
            Lexicon lexicon = Lexicon.DefaultLexicon;
            NLGFactory nlgFactory = new NLGFactory(lexicon);
            Realiser realiser = new Realiser(lexicon);
            realiser.Formatter = new HTMLFormatter();
            DocumentElement document = nlgFactory.createDocument("Document");
            DocumentElement paragraph = nlgFactory.createParagraph();
            DocumentElement list = nlgFactory.createEnumeratedList();
            DocumentElement item1 = nlgFactory.createListItem();
            DocumentElement item2 = nlgFactory.createListItem();


            DocumentElement sentence1 = nlgFactory.createSentence("this", "be", "the first sentence");
            DocumentElement sentence2 = nlgFactory.createSentence("this", "be", "the second sentence");
            item1.addComponent(sentence1);
            item2.addComponent(sentence2);
            list.addComponent(item1);
            list.addComponent(item2);
            paragraph.addComponent(list);
            document.addComponent(paragraph);
            string expectedOutput = "<h1>Document</h1>" + "<p>" + "<ol>" + "<li>This is the first sentence.</li>"
                                    + "<li>This is the second sentence.</li>" + "</ol>" + "</p>";

            string realisedOutput = realiser.realise(document).Realisation;


            Assert.AreEqual(expectedOutput, realisedOutput);
        }
    }
}