﻿using System;
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
        public string? Source { get; set; }

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
                Source = Source,
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
                        drive.Source = null;
                    
                    drive.Type = childDrive.Type ?? drive.Type;
                    drive.Source = childDrive.Source ?? drive.Source;
                    
                    if (childDrive.Size != 0) drive.Size = childDrive.Size;
                    if (!string.IsNullOrWhiteSpace(childDrive.Label))
                        drive.Label = childDrive.Label;
                    if (!string.IsNullOrWhiteSpace(childDrive.Lair))
                        drive.Lair = childDrive.Lair;               
                },
                (drive) =>
                {
                    if(string.IsNullOrWhiteSpace(drive.Source))
                    {
                        drive.Source = $"{parentReference}:{drive.Name}";
                    }
                }
            );
            
         }
    }
}