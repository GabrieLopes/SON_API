using System.Collections.Generic;

namespace API_REST.HATEOAS
{
    public class HATEOAS
    {
        private string url;
        private string protocol = "https://";
        public List<Link> actions = new List<Link>();

        public HATEOAS(string url)
        {
            this.url = url;
        }

        public HATEOAS(string url, string protocol)
        {
            this.url = url;
            this.protocol = protocol;
        }

        public void AddAction(string rel, string method)
        {
            // https:// localhost:5001/api/v1/Produtos
            actions.Add(new Link(this.protocol + this.url, rel, method));
        }

        public Link[] GetActions(string sufix)
        {
            //AspNet traduz mais consistentemente um Array do que um ArrayList
            //Clonando valores do objeto
            Link[] tempLinks = new Link[actions.Count];
            for(int i=0; i < tempLinks.Length; i++)
            {
                tempLinks[i] = new Link(actions[i].href, actions[i].rel, actions[i].method);
            }

            /* Montagem do link */
            foreach (var link in tempLinks)
            {   // https:// localhost:5001/api/v1/Produtos/ 2/10/gabriel
                link.href = link.href + "/" + sufix.ToString();
            }
            return tempLinks;
        }
    }
}