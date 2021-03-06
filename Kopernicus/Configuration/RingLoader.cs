﻿/** 
 * Kopernicus Planetary System Modifier
 * Copyright (C) 2014 Bryce C Schroeder (bryce.schroeder@gmail.com), Nathaniel R. Lewis (linux.robotdude@gmail.com)
 * 
 * http://www.ferazelhosting.net/~bryce/contact.html
 * 
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston,
 * MA 02110-1301  USA
 * 
 * This library is intended to be used as a plugin for Kerbal Space Program
 * which is copyright 2011-2014 Squad. Your usage of Kerbal Space Program
 * itself is governed by the terms of its EULA, not the license above.
 * 
 * Code based on KittiopaTech, modified by Thomas P.
 * 
 * https://kerbalspaceprogram.com
 */

using System.Collections.Generic;

using UnityEngine;
using Kopernicus.Configuration.Resources;

namespace Kopernicus
{
	namespace Configuration
	{
		[RequireConfigType(ConfigType.Node)]
        public class RingLoader : IParserEventSubscriber
        {
            // Set-up our custom ring
            public Ring ring;

            // Our Scaled Planet
            public GameObject ScaledPlanet { get; set; }
            
            // Inner Radius of our ring
            [ParserTarget("innerRadius", optional = true, allowMerge = false)]
            public NumericParser<double> innerRadius
            {
                set { ring.innerRadius = value.value; }
            }

            // Outer Radius of our ring
            [ParserTarget("outerRadius", optional = true, allowMerge = false)]
            public NumericParser<double> outerRadius
            {
                set { ring.outerRadius = value.value; }
            }

            // Axis angle of our ring
            [ParserTarget("angle", optional = true, allowMerge = false)]
            public NumericParser<float> angle
            {
                set { ring.angle = value.value; }
            }

            // Texture of our ring
            [ParserTarget("texture", optional = true, allowMerge = false)]
            public Texture2DParser texture
            {
                set { ring.texture = value.value; }
            }

            // Color of our ring
            [ParserTarget("color", optional = true, allowMerge = false)]
            public ColorParser color
            {
                set { ring.color = value.value; }
            }

            // Lock rotation of our ring?
            [ParserTarget("lockRotation", optional = true, allowMerge = false)]
            public NumericParser<bool> lockRotation 
            {
                set { ring.lockRotation = value.value; } 
            }

            // Unlit our ring?
            [ParserTarget("unlit", optional = true, allowMerge = false)]
            public NumericParser<bool> unlit
            {
                set { ring.unlit = value.value; }
            }


            // Initialize the RingLoader
            public RingLoader()
            {
                ring = new Ring();
            }

            // Apply event
            void IParserEventSubscriber.Apply(ConfigNode node)
            { 
            }

            // Post-Apply event
            void IParserEventSubscriber.PostApply(ConfigNode node)
            {
                
            }

            // Rings
            public static void AddRing(GameObject ScaledPlanet, Ring ring)
            {
                Logger.Active.Log("Adding Ring to " + ScaledPlanet.name);
                Vector3 StartVec = new Vector3(1, 0, 0);
                int RingSteps = 128;
                var vertices = new List<Vector3>();
                var Uvs = new List<Vector2>();
                var Tris = new List<int>();
                var Normals = new List<Vector3>();
                
                for (float i = 0.0f; i < 360.0f; i += (360.0f / RingSteps))
                {
                    var eVert = Quaternion.Euler(0, i, 0) * StartVec;

                    //Inner Radius
                    vertices.Add(eVert * (float)ring.innerRadius);
                    Normals.Add(-Vector3.right);
                    Uvs.Add(new Vector2(0, 0));

                    //Outer Radius
                    vertices.Add(eVert * (float)ring.outerRadius);
                    Normals.Add(-Vector3.right);
                    Uvs.Add(new Vector2(1, 1));
                }
                
                for (float i = 0.0f; i < 360.0f; i += (360.0f / RingSteps))
                {
                    var eVert = Quaternion.Euler(0, i, 0) * StartVec;

                    //Inner Radius
                    vertices.Add(eVert * (float)ring.innerRadius);
                    Normals.Add(-Vector3.right);
                    Uvs.Add(new Vector2(0, 0));

                    //Outer Radius
                    vertices.Add(eVert * (float)ring.outerRadius);
                    Normals.Add(-Vector3.right);
                    Uvs.Add(new Vector2(1, 1));
                }
                
                //Tri Wrapping
                int Wrapping = (RingSteps * 2);
                for (int i = 0; i < (RingSteps * 2); i += 2)
                {
                    Tris.Add((i) % Wrapping);
                    Tris.Add((i + 1) % Wrapping);
                    Tris.Add((i + 2) % Wrapping);

                    Tris.Add((i + 1) % Wrapping);
                    Tris.Add((i + 3) % Wrapping);
                    Tris.Add((i + 2) % Wrapping);
                }
                
                for (int i = 0; i < (RingSteps * 2); i += 2)
                {
                    Tris.Add(Wrapping + (i + 2) % Wrapping);
                    Tris.Add(Wrapping + (i + 1) % Wrapping);
                    Tris.Add(Wrapping + (i) % Wrapping);

                    Tris.Add(Wrapping + (i + 2) % Wrapping);
                    Tris.Add(Wrapping + (i + 3) % Wrapping);
                    Tris.Add(Wrapping + (i + 1) % Wrapping);
                }
                
                //Create GameObject
                GameObject RingObject = new GameObject("PlanetaryRingObject");
                RingObject.transform.parent = ScaledPlanet.transform;
                RingObject.transform.position = ScaledPlanet.transform.localPosition;
                RingObject.transform.localRotation = Quaternion.Euler(ring.angle, 0, 0);

                RingObject.transform.localScale = ScaledPlanet.transform.localScale;
                RingObject.layer = ScaledPlanet.layer;
                
                //Create MeshFilter
                MeshFilter RingMesh = (MeshFilter)RingObject.AddComponent<MeshFilter>();
                
                //Set mesh
                RingMesh.mesh = new Mesh();
                RingMesh.mesh.vertices = vertices.ToArray();
                RingMesh.mesh.triangles = Tris.ToArray();
                RingMesh.mesh.uv = Uvs.ToArray();
                RingMesh.mesh.RecalculateNormals();
                RingMesh.mesh.RecalculateBounds();
                RingMesh.mesh.Optimize();
                RingMesh.sharedMesh = RingMesh.mesh;

                //Set texture
                //MeshRenderer PlanetRenderer = (MeshRenderer)ScaledPlanet.GetComponentsInChildren<MeshRenderer>()[0]; 
                MeshRenderer RingRender = (MeshRenderer)RingObject.AddComponent<MeshRenderer>();
                RingRender.material = ScaledPlanet.renderer.material;
                
                if (ring.unlit)
                {
                    Material material = new Material(Shaders.UnlitNew);
                    RingRender.material = material;
                }
                else
                {
                    Material material = new Material(Shaders.DiffuseNew);
                    RingRender.material = material;
                }

                RingRender.material.mainTexture = ring.texture;
                RingRender.material.color = ring.color;

                ScaledPlanet.renderer.material.renderQueue = 1;
                RingRender.material.renderQueue = 2;

                RingObject.AddComponent<ReScaler>();

                if (ring.lockRotation)
                {
                    Quaternion m_rotAngleLock = RingObject.transform.localRotation;
                    AngleLocker m_ringAngleLock = (AngleLocker)RingObject.AddComponent<AngleLocker>();
                    m_ringAngleLock.RotationLock = m_rotAngleLock;
                }

                GameObject.DontDestroyOnLoad(RingObject);
            }

        }

        //=====================================//
        //          Helper classes!            //
        //=====================================//

        public class Ring
        {
            public double innerRadius { get; set; }
            public double outerRadius { get; set; }
            public float angle { get; set; }
            public Texture2D texture { get; set; }
            public Color color { get; set; }
            public bool lockRotation { get; set; }
            public bool unlit { get; set; }
        }

        public class AngleLocker : MonoBehaviour
        {
            public Quaternion RotationLock;

            void Update()
            {
                transform.rotation = RotationLock;
            }
            void FixedUpdate()
            {
                transform.rotation = RotationLock;
            }
            void LateUpdate()
            {
                transform.rotation = RotationLock;
            }
        }

        public class ReScaler : MonoBehaviour
        {
            void Update()
            {
                transform.localScale = transform.parent.localScale;
            }
            void FixedUpdate()
            {
                transform.localScale = transform.parent.localScale;
            }
            void LateUpdate()
            {
                transform.localScale = transform.parent.localScale;
            }
        }
    }
}
