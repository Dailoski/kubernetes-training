# Exercise 1: Setting Up Your Local Kubernetes Environment

Before running workloads on Kubernetes, you need a cluster and the right tools to interact with it. In this exercise, you will set up a local Kubernetes cluster, install required CLI tools, and verify access.

## Pre-requisites

- [Docker](https://docs.docker.com/get-docker/) installed (for Docker Desktop or Minikube driver)  
- Basic terminal/command-line knowledge  

---

## Instructions

### 1. Choose a Local Kubernetes Option

You can run Kubernetes locally in different ways. Choose **one** of the following:

#### Option A: Docker Desktop (MacOS/Windows/Linux)
1. Enable Kubernetes in Docker Desktop:  
   - Open Docker Desktop → Preferences → Kubernetes → check **Enable Kubernetes**.  
2. Apply changes and wait until the Kubernetes status shows as "Running".

#### Option B: Minikube (cross-platform)
1. Install Minikube:  
   - [Installation guide](https://minikube.sigs.k8s.io/docs/start/)  
2. Start a cluster:  

   ~~~bash
   minikube start --driver=docker
   ~~~

3. Verify the cluster is running:  

   ~~~bash
   kubectl get nodes
   ~~~

---

### 2. Install kubectl

`kubectl` is the Kubernetes command-line tool.  

Follow the [official install guide](https://kubernetes.io/docs/tasks/tools/#kubectl).  

Verify installation:  

~~~bash
kubectl version --client
~~~

---

### 3. Install k9s (Optional but Recommended)

[k9s](https://k9scli.io/) provides a terminal UI to interact with Kubernetes.  

Install it:  

- **MacOS (Homebrew):**  
  ~~~bash
  brew install derailed/k9s/k9s
  ~~~

- **Linux (snap):**  
  ~~~bash
  sudo snap install k9s
  ~~~

- **Windows (scoop):**  
  ~~~powershell
  scoop install k9s
  ~~~

Run k9s to check:  

~~~bash
k9s
~~~

---

### 4. Verify Cluster Access

Check that your kubeconfig file is set up correctly:  

~~~bash
kubectl config view
kubectl config current-context
kubectl config get-contexts
~~~

If you see a context (for example `docker-desktop` or `minikube`) and it matches your cluster, kubeconfig is configured.

Check nodes in your cluster:  

~~~bash
kubectl get nodes
~~~

Check system pods in `kube-system` namespace:  

~~~bash
kubectl get pods -n kube-system
~~~

If you see running pods and a Ready node, your cluster is set up.

---

### Optional Exercises

- Try starting Minikube with more resources:  
  ~~~bash
  minikube start --cpus=2 --memory=4096
  ~~~

- View Kubernetes dashboard with Minikube:  
  ~~~bash
  minikube dashboard
  ~~~

- Switch between clusters if you have multiple:  
  ~~~bash
  kubectl config use-context <name>
  ~~~

---

## Summary

In this exercise, you learned how to:  
- Set up a local Kubernetes cluster (Docker Desktop or Minikube).  
- Install and configure `kubectl`.  
- Inspect your kubeconfig and current context.  
- Optionally install `k9s` for easier management.  
- Verify access by listing nodes and system pods.  

For more information:  
- [Docker Desktop Kubernetes](https://docs.docker.com/desktop/kubernetes/)  
- [Minikube Documentation](https://minikube.sigs.k8s.io/docs/start/)  
- [kubectl Cheat Sheet](https://kubernetes.io/docs/reference/kubectl/cheatsheet/)  
