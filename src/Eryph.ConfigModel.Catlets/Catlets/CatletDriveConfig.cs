using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletDriveConfig: IMutateableConfig<CatletDriveConfig>, ICloneable
    {
        public string? Name { get; set; }
        
        public MutationType? Mutation { get; set; }
        
        public string? Label { get; set; }
        public string? Lair { get; set; }

        [PrivateIdentifier]
        public string? Parent { get; set; }

        public int? Size { get; set; }
        public CatletDriveType? Type { get; set; }

        public CatletDriveConfig Clone()
        {
            return new CatletDriveConfig
            {
                Name = Name,
                Mutation = Mutation,
                Label = Label,
                Lair = Lair,
                Parent = Parent,
                Size = Size,
                Type = Type
            };
        }
        
        object ICloneable.Clone()
        {
            return Clone();
        }

        internal static CatletDriveConfig[]? Breed(CatletConfig parentConfig, CatletConfig child, 
            string? parentReference)
        {
            return Breeding.WithMutation(parentConfig, child, x => x.Drives,
                (drive, childDrive) =>
                {

                    if (childDrive.Type.HasValue && drive.Type != childDrive.Type)
                        drive.Parent = null;
                    
                    drive.Type = childDrive.Type ?? drive.Type;
                    drive.Parent = childDrive.Parent ?? drive.Parent;
                    
                    if (childDrive.Size != 0) drive.Size = childDrive.Size;
                    if (!string.IsNullOrWhiteSpace(childDrive.Label))
                        drive.Label = childDrive.Label;
                    if (!string.IsNullOrWhiteSpace(childDrive.Lair))
                        drive.Lair = childDrive.Lair;               
                },
                (drive) =>
                {
                    if(string.IsNullOrWhiteSpace(drive.Parent))
                    {
                        drive.Parent = $"{parentReference}:{drive.Name}";
                    }
                }
            );
            
         }
    }
}