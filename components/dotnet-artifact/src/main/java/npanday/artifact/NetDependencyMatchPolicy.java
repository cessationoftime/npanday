/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
package npanday.artifact;

import npanday.model.netdependency.NetDependency;

/**
 * Provides a way for a developer to implement custom match policies for a net dependency.
 *
 * @author Shane Isbell
 */
public interface NetDependencyMatchPolicy
{
    /**
     * Return true if the policy matches the specified net dependency, otherwise returns false.
     *
     * @param netDependency the net dependency to match
     * @return true if the policy matches the specified net dependency, otherwise returns false
     */
    boolean match( NetDependency netDependency );
}
