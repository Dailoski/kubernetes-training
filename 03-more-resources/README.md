# Exercise 3: ConfigMaps, Secrets, Load Balancing, Probes, and StatefulSets

In this exercise, you will learn how to:

- Store configuration outside containers using **ConfigMaps** and **Secrets**  
- Inject them as environment variables  
- Demonstrate **load balancing** with multiple replicas  
- Use **readiness and liveness probes** for health checks  
- Deploy a **StatefulSet** for workloads that need stable identity and storage  

---

## Pre-requisites

- [kubectl](https://kubernetes.io/docs/tasks/tools/) installed and configured  
- A running Kubernetes cluster (Docker Desktop, Minikube, k3s, or kind)  

---

## Step 1: Create a ConfigMap

~~~bash
kubectl create configmap app-config \
  --from-literal=APP_ENV=development \
  --from-literal=APP_MESSAGE="Hello from ConfigMap"
~~~

Verify:

~~~bash
kubectl get configmap app-config -o yaml
~~~

---

## Step 2: Create a Secret

~~~bash
kubectl create secret generic app-secret \
  --from-literal=DB_PASSWORD=mysecretpassword
~~~

Verify:

~~~bash
kubectl get secret app-secret -o yaml
~~~

---

## Step 3: Deploy an Application with ConfigMap, Secret, and Probes

Create `configmap-secret-deploy.yaml`:

~~~yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: configmap-demo
spec:
  replicas: 3
  selector:
    matchLabels:
      app: configmap-demo
  template:
    metadata:
      labels:
        app: configmap-demo
    spec:
      containers:
        - name: demo-app
          image: hashicorp/http-echo
          args:
            - "-text=$(APP_MESSAGE)"
          env:
            - name: APP_ENV
              valueFrom:
                configMapKeyRef:
                  name: app-config
                  key: APP_ENV
            - name: APP_MESSAGE
              valueFrom:
                configMapKeyRef:
                  name: app-config
                  key: APP_MESSAGE
            - name: DB_PASSWORD
              valueFrom:
                secretKeyRef:
                  name: app-secret
                  key: DB_PASSWORD
          ports:
            - containerPort: 5678
          readinessProbe:
            httpGet:
              path: /
              port: 5678
            initialDelaySeconds: 5
            periodSeconds: 5
          livenessProbe:
            httpGet:
              path: /
              port: 5678
            initialDelaySeconds: 10
            periodSeconds: 10
---
apiVersion: v1
kind: Service
metadata:
  name: configmap-demo
spec:
  type: NodePort
  selector:
    app: configmap-demo
  ports:
    - port: 5678
      targetPort: 5678
~~~

Apply it:

~~~bash
kubectl apply -f configmap-secret-deploy.yaml
kubectl get pods
kubectl describe pod <pod-name>
~~~

---

## Step 4: Test Load Balancing

Get the Service details:

~~~bash
kubectl get svc configmap-demo
~~~

Access the service multiple times:

~~~bash
curl http://<node-ip>:<node-port>
curl http://<node-ip>:<node-port>
curl http://<node-ip>:<node-port>
~~~

You should see responses from different Pods.

---

## Step 5: Inspect Environment Variables in a Pod

~~~bash
kubectl get pods
kubectl exec -it <pod-name> -- printenv | grep APP
kubectl exec -it <pod-name> -- printenv | grep DB_PASSWORD
~~~

---

## Step 6: Deploy a StatefulSet

Create `nginx-statefulset.yaml`:

~~~yaml
apiVersion: apps/v1
kind: StatefulSet
metadata:
  name: nginx-stateful
spec:
  serviceName: "nginx"
  replicas: 2
  selector:
    matchLabels:
      app: nginx-stateful
  template:
    metadata:
      labels:
        app: nginx-stateful
    spec:
      containers:
        - name: nginx
          image: nginx:stable-alpine
          ports:
            - containerPort: 80
  volumeClaimTemplates:
    - metadata:
        name: www
      spec:
        accessModes: [ "ReadWriteOnce" ]
        resources:
          requests:
            storage: 1Gi
~~~

Apply:

~~~bash
kubectl apply -f nginx-statefulset.yaml
kubectl get pods -l app=nginx-stateful
kubectl get pvc
~~~

Notice pods named `nginx-stateful-0`, `nginx-stateful-1`, each with its own PVC.

---

## Optional Exercises

- Break the readiness probe (set `path: /wrong`) and see what happens  
- Scale the Deployment up and down with `kubectl scale`  
- Delete and recreate the StatefulSet — notice PVCs are retained  

---
