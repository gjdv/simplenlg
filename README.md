[![Build Status](https://travis-ci.org/gjdv/simplenlg.svg?branch=master)](https://travis-ci.org/gjdv/simplenlg)

SimpleNLG fork by GJdV
======================

This fork is based upon [SimpleNLG](https://github.com/simplenlg/simplenlg). The following changes have been made:

Additions
---------

* Addition of feature "IS_CAPITALIZED" to allow storing of (first letter) capitalization of words, decapitalizing them during the processing (realisation) and capitalizing them again during orthographic realisation. This can come in handy when using (capitalized) names, while requiring correct pluralization.
* Port of SimpleNLG to C#. 


Port to C#
----------
This port to C# was made for better integration in C# projects. Note that the C# code could be optimized further, however, it was chosen to stay close to the original to aid in maintenance when updates are made to the original SimpleNLG code. 
The ported unit tests were used to verify correctness of the port. Currently only the server test fails (see "Known Issues").
In order to remove all dependency to java, the HSQLdb containing the NIHDB lexicon was converted to SQLITE. If there is a way to interact from C# directly (without IKVM or other wrappers) to HSQLdb, please let me know. There are some placeholders (outcommented) in the code to accomodate for this functionality. To identify which mode (HSQL or SQLITE) to run, the lexicon-type (used e.g., in lexicon.properties) has been extended with the options NIH_SQLITE and NIH_HSQL; the original value NIH defaults to NIH_HSQL. These SQLITE databases are located in \SimpleNLG\srcCsharp\Resources\NIHLexicon as zip-files and should be extracted before running the code.

This port also includes the sources of [LexAccess] (https://lexsrv3.nlm.nih.gov/LexSysGroup/Projects/lexAccess/current/web/download.html) and some dependencies from [Lexical Tools] (https://lexsrv3.nlm.nih.gov/LexSysGroup/Projects/lvg/current/web/index.html).
Various versions of [NIH Lexicon] (https://lsg3.nlm.nih.gov/LexSysGroup/Projects/lexicon/current/web/index.html) were converted from HSQLdb to SQLITE.


Known issues
------------

* Client/Server setup of SimpleNLG does not work (and fails the unit test); this unit test is disabled; 
* NIHDB lexicon functionality only available through the converted SQLITE databases; No proper way identified (yet) to directly use HSQLdb directly from C#

&nbsp;

Below you can find the original README from SimpleNLG:

&nbsp;

&nbsp;

___

&nbsp;


SimpleNLG
=========

SimpleNLG is a simple Java API designed to facilitate the generation of Natural Language. It was originally developed by Ehud Reiter, Professor at the [University of Aberdeen's Department of Computing Science](http://www.abdn.ac.uk/ncs/departments/computing-science/index.php) and co-founder of [Arria NLG](https://www.arria.com). The [discussion list for SimpleNLG is on Google Groups](https://groups.google.com/forum/#!forum/simplenlg).

SimpleNLG is intended to function as a "[realisation engine](http://en.wikipedia.org/wiki/Realization_(linguistics))" for [Natural Language Generation](http://en.wikipedia.org/wiki/Natural_language_generation) architectures, and has been used successfully in a number of projects, both academic and commercial. It handles the following:

* Lexicon/morphology system: The default lexicon computes inflected forms (morphological realisation). We believe this has fair coverage. Better coverage can be obtained by using the [NIH Specialist Lexicon](http://lexsrv3.nlm.nih.gov/LexSysGroup/Projects/lexicon/current/web/) (which is supported by SimpleNLG).
* Realiser: Generates texts from a syntactic form. Grammatical coverage is limited compared to tools such as [KPML](http://www.fb10.uni-bremen.de/anglistik/langpro/kpml/README.html) and [FUF/SURGE](http://www.cs.bgu.ac.il/surge/index.html), but we believe it is adequate for many NLG tasks.
* Microplanning: Currently just simple aggregation, hopefully will grow over time.

Current release (English)
-------------------------
The current release of SimpleNLG is V4.4.8 ([API](https://cdn.rawgit.com/simplenlg/simplenlg/master/docs/javadoc/index.html)). The "official" version of SimpleNLG only produces texts in English. However, versions for other languages are under development, see the Papers and Publications page and [SimpleNLG discussion list](https://groups.google.com/forum/#!forum/simplenlg) for details.

Please note that earlier versions of SimpleNLG have different licensing, in particular versions before V4.0 cannot be used commercially.

Getting started
---------------
For information on how to use SimpleNLG, please see the [tutorial](https://github.com/simplenlg/simplenlg/wiki/Section-0-–-SimpleNLG-Tutorial) and [API](https://cdn.rawgit.com/simplenlg/simplenlg/master/docs/javadoc/index.html).

If you have a technical question about using SimpleNLG, please check the [SimpleNLG discussion list](https://groups.google.com/forum/#!forum/simplenlg).

If you wish to be informed about SimpleNLG updates and events, please subscribe to the [SimpleNLG announcement list](https://groups.google.com/forum/#!forum/simplenlg-announce).

If you wish to cite SimpleNLG in an academic publication, please cite the following paper:

* A Gatt and E Reiter (2009). [SimpleNLG: A realisation engine for practical applications](http://aclweb.org/anthology/W/W09/W09-0613.pdf). Proceedings of ENLG-2009

If you have other questions about SimpleNLG, please contact Professor Ehud Reiter via email: [ehud.reiter@arria.com](mailto:ehud.reiter@arria.com).

SimpleNLG for other languages
-----------------------------
A version of SimpleNLG has now been developed for *French* by *Pierre-Luc Vaudry* at the Université de Montreal. This is a development based on the version 4 architecture. It is current being distributed separately from [this page](http://www-etud.iro.umontreal.ca/~vaudrypl/snlgbil/snlgEnFr_english.html).

*Marcel Bollman* has been working on an adaptation of SimpleNLG version 3 to German. This is available from [this page](http://www.linguistics.rub.de/~bollmann/simplenlg-ger.html). Please remember that SimpleNLG version 3 is not licensed for commercial use

SimpleNLG License 
-----------------------------
SimpleNLG is licensed under the terms and conditions of the [Mozilla Public Licence (MPL)](http://www.mozilla.org/MPL/).
