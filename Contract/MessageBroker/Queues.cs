using System;
using System.Collections.Generic;
using System.Text;

namespace Contract.MessageBroker
{
    public class Queues
    {
        public static string ANIMAL_CREATION_QUEUE = "Animal.Creation.Queue";
        public static string ANIMAL_MEDIA_ADDED_QUEUE = "Animal.Image.Added.Queue";
        public static string ANIMAL_PROFILE_ADDED_QUEUE = "Animal.Profile.Added.Queue";
        public static string ANIMAL_MEDIA_COMPUTED = "Animal.Image.Computed.Queue";
        public static string ANIMAL_PROFILE_COMPUTED = "Animal.Profile.Computed.Queue";
        public static string ANIMAL_COMPUTED_STATISTICS = "Animal.Computed.Statistics";
    }
}
