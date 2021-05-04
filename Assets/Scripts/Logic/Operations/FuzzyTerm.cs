using System.Collections.Generic;
using System.Xml.Serialization;

namespace Chess
{
namespace AI
{

[XmlInclude(typeof(FT_Set)), XmlInclude(typeof(FT_And)), XmlInclude(typeof(FT_Fairly)), XmlInclude(typeof(FT_Not)), XmlInclude(typeof(FT_Or)), XmlInclude(typeof(FT_Very)), ]
public abstract class FuzzyTerm {

    // all of the specific operations will be defined as terms
    // and the only requirement is that they can output a confidence
    // value for other terms to ingest
    // the outermost fuzzy term which outputs its confidence will therefore
    // output the confidence of the whole sentence
    public abstract double compute_confidence();

}

} // AI
} // Chess