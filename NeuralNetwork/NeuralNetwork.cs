﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HNS.NeuralNetwork
{
    internal class NeuralNetwork
    {
        //Layers
        public List<Neuron> InputsLayer { get; }
        public List<List<Neuron>> HiddenLayers { get; }
        public List<Neuron> OutputsLayer { get; }

        /// <summary>
        /// Initialization SeekerNeuroNetwork
        /// </summary>
        public NeuralNetwork(int InputsNum, List<int> HiddenLayersNum, int OutputNum, bool Generate = true)
        {
            InputsLayer = new List<Neuron>();
            HiddenLayers = new List<List<Neuron>>();
            for (int i = 0; i < HiddenLayersNum.Count; i++)
            {
                HiddenLayers.Add(new List<Neuron>());
            }
            OutputsLayer = new List<Neuron>();

            if (Generate) InitializationLayers(InputsNum, HiddenLayersNum, OutputNum);
        }

        /// <summary>
        /// Initialization layers by the number of neurons of each layer
        /// </summary>
        /// <param name="InputsNum">Number of inputs neurons</param>
        /// <param name="hiddenLayers">list of hidden layers neurons</param>
        /// <param name="OutputNum">Number of outputs neurons</param>
        private void InitializationLayers(int InputsNum, List<int> hiddenLayers, int OutputNum)
        {
            // Initialization input layer
            for (int i = 0; i < InputsNum; i++)
            {
                InputsLayer.Add(new Neuron());
            }
            // Initialization hidden layer
            for (int i = 0; i < hiddenLayers.Count; i++)
            {
                for (int j = 0; j < hiddenLayers[i]; j++)
                {
                    double Bais = GetRandomWeight();
                    List<Neuron> prevLayer = i == 0 ? InputsLayer : HiddenLayers[i - 1];
                    HiddenLayers[i].Add(new Neuron(Bais, NeuronTypes.HIDDEN, prevLayer.Count));
                }
            }
            // Initialization output layer
            for (int i = 0; i < OutputNum; i++)
            {
                OutputsLayer.Add(new Neuron(0, NeuronTypes.OUTPUT, hiddenLayers[hiddenLayers.Count - 1]));
            }
        }

        /// <summary>
        /// Return random double number between -1 to 1
        /// </summary>
        /// <returns>number between -1 to 1</returns>
        public static double GetRandomWeight()
        {
            return StaticClass.rnd.NextDouble() * 2 - 1;
        }

        /// <summary>
        /// Set next layer bais
        /// </summary>
        /// <param name="PreviousLayer">List of previous layer</param>
        /// <param name="NextLayer">List of next layer/param>
        private void SetNextLyerBias(List<Neuron> PreviousLayer, List<Neuron> NextLayer)
        {
            foreach (Neuron NextNeuron in NextLayer)
            {
                double sum = 0.0;
                for (int i = 0; i < PreviousLayer.Count; i++)
                {
                    sum += PreviousLayer[i].NBias * NextNeuron.NWeights[i];
                }
                // activation function
                NextNeuron.NBias = 1.0f / (1.0f + (float)Math.Exp(-sum));
            }
        }

        /// <summary>
        /// set all layers baises
        /// </summary>
        /// <param name="Inputs">Neuron list of input layer</param>
        public void SetAllLayerBias(List<double> Inputs)
        {
            // Set Bais of input layers
            for (int i = 0; i < Inputs.Count; i++)
            {
                InputsLayer[i].NBias = Inputs[i];
            }
            // set first hidden layer bais
            SetNextLyerBias(InputsLayer, HiddenLayers[0]);
            // set other hidden layer bais
            for (int i = 1; i < HiddenLayers.Count; i++)
            {
                SetNextLyerBias(HiddenLayers[i - 1], HiddenLayers[i]);
            }
            //set output layer bais
            SetNextLyerBias(HiddenLayers[HiddenLayers.Count - 1], OutputsLayer);
        }

        /// <summary>
        /// Change neuron weights values
        /// </summary>
        /// <param name="ShakingRate">Shaking rate</param>
        public void ChangeNeuronWeights(double ShakingRate)
        {
            // Change Hidden Layer weights values
            for (int i = 0; i < HiddenLayers.Count; i++)
            {
                for (int j = 0; j < HiddenLayers[i].Count; j++)
                {
                    for (int w = 0; w < HiddenLayers[i][j].NWeights.Length; w++)
                    {
                        if (StaticClass.rnd.NextDouble() < ShakingRate)
                        {
                            HiddenLayers[i][j].NWeights[w] = GetRandomWeight();
                        }
                    }
                }
            }
            // Change Output Layer weights values
            for (int i = 0; i < OutputsLayer.Count; i++)
            {
                for (int w = 0; w < OutputsLayer[i].NWeights.Length; w++)
                {
                    if (StaticClass.rnd.NextDouble() < ShakingRate)
                    {
                        OutputsLayer[i].NWeights[w] = GetRandomWeight();
                    }
                }
            }
        }

        /// <summary>
        /// Return copy of neural network
        /// </summary>
        /// <returns>copy of neural network</returns>
        public NeuralNetwork Copy()
        {
            List<int> hidden = new List<int>();
            for (int i = 0; i < HiddenLayers.Count; i++)
                hidden.Add(HiddenLayers[i].Count);
            var result = new NeuralNetwork(InputsLayer.Count, hidden, OutputsLayer.Count, false);
            for (int i = 0; i < InputsLayer.Count; i++)
            {
                result.InputsLayer.Add(InputsLayer[i].Copy());
            }
            for (int i = 0; i < hidden.Count; i++)
            {
                for (int j = 0; j < hidden[i]; j++)
                {
                    result.HiddenLayers[i].Add(HiddenLayers[i][j].Copy());
                }
            }
            for (int i = 0; i < OutputsLayer.Count; i++)
            {
                result.OutputsLayer.Add(OutputsLayer[i].Copy());
            }
            return result;
        }

        /// <summary>
        /// Crossing two neuronal networks
        /// </summary>
        /// <param name="other">Second neural network</param>
        public void Cross(NeuralNetwork other)
        {
            for (int i = 0; i < HiddenLayers.Count; i++)
            {
                for (int j = 0; j < HiddenLayers[i].Count; j++)
                {
                    if (StaticClass.rnd.NextDouble() > 0.5) 
                        HiddenLayers[i][j] = other.HiddenLayers[i][j].Copy();
                }
            }
            for (int j = 0; j < OutputsLayer.Count; j++)
            {
                if (StaticClass.rnd.NextDouble() > 0.5) 
                    OutputsLayer[j] = other.OutputsLayer[j].Copy();
            }
        }
    }
}
