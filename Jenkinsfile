pipeline {
    agent any

    environment {
        IMAGE_NAME = 'YOUR_USER/currency-api'
        TAG = 'latest'
    }

    stages {
        stage('Checkout') {
            steps {
                git url: 'https://github.com/SixteenMissedCalls/WebApi-test-project.git', branch: 'prod'
            }
        }

        stage('Build') {
            steps {
                sh 'dotnet restore Src/CurrencyRateGateway'
                sh 'dotnet publish Src/CurrencyRateGateway -c Release -o publish'
            }
        }

        stage('Docker Build') {
            steps {
                sh 'docker build -t $IMAGE_NAME:$TAG -f Src/CurrencyRateGateway/Dockerfile .'
            }
        }

        stage('Docker Login & Push') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'docker-hub-creds', usernameVariable: 'YOUR_DOCKER_USER', passwordVariable: 'YOUR_DOCKER_PASS')]) {
                    sh 'echo "$YOUR_DOCKER_PASS" | docker login -u "$YOUR_DOCKER_USER" --password-stdin'
                    sh 'docker push $IMAGE_NAME:$TAG'
                }
            }
        }

        stage('Cleanup') {
            steps {
                sh 'docker rmi $IMAGE_NAME:$TAG || true'
            }
        }
    }
}
